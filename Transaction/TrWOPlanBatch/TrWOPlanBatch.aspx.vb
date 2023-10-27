Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Partial Class Transaction_TrWOPLanBatch_TrWOPLanBatch
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT DISTINCT TransNmbr, TransDate, Status, Divisi, DivisiName, WorkBy, WorkByName, ReffType, Reference, MonthNo, JobPlant, JobPlantName, Person, QtyBlok, Qty, Unit, UserPrep, DatePrep, UserAppr, DateAppr, " + _
                " Supplier, SupplierName, CIP, CIPName, EstStartWeek, EstEndWeek, QtyWeek, Remark FROM V_PLWOPlanBacthHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Menu1.Visible = False
                Session("AdvanceFilter") = ""
                'lbCount.Text = SQLExecuteScalar("SELECT COUNT(TransNmbr) FROM V_PLWOPlanGetReff WHERE REFF_Type = 'Batch'", ViewState("DBConnection").ToString)

                lbCount.Text = SQLExecuteScalar("SELECT COUNT(A.TransNmbr) FROM V_PLWOPlanGetReff A LEFT OUTER JOIN PLWOPlanHd B ON A.TransNmbr = B.Reference And A.JobPlant = B.JobPlant And A.Rotasi = B.MonthNo WHere B.Status Is NULL And REFF_Type = 'Batch'", ViewState("DBConnection").ToString)
                btnGetData.Visible = False
            End If

            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnReff" Then
                    'TransNmbr, JobPlant, JobPlant_Name, Rotasi, Team, Person
                    BindToText(tbReffNo, Session("Result")(0).ToString)
                    BindToText(tbJob, Session("Result")(1).ToString)
                    BindToText(tbJobName, Session("Result")(2).ToString)
                    BindToText(tbRotasi, Session("Result")(3).ToString)
                    BindToDropList(ddlWorkBy, Session("Result")(4).ToString)
                    BindToText(tbUnit, Session("Result")(5).ToString)
                    tbKontraktor.Enabled = False
                    btnKontraktor.Enabled = False
                    '   AttachScript("setformatdt()", Page, Me.GetType())
                End If
                If ViewState("Sender") = "btnJob" Then
                    BindToText(tbJob, Session("Result")(0).ToString)
                    BindToText(tbJobName, Session("Result")(1).ToString)
                    BindToText(tbUnit, Session("Result")(2).ToString)
                    If Session("Result")(2).ToString = "Y" Then
                        tbCIP.Enabled = True
                    Else
                        tbCIP.Enabled = False
                    End If
                    btnCIP.Enabled = tbCIP.Enabled
                End If
                If ViewState("Sender") = "btnCIP" Then
                    BindToText(tbCIP, Session("Result")(0).ToString)
                    BindToText(tbCIPName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnKontraktor" Then
                    BindToText(tbKontraktor, Session("Result")(0).ToString)
                    BindToText(tbKontraktorName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnStart" Then
                    BindToText(tbStart, Session("Result")(0).ToString)
                    BindToText(tbEnd, Session("Result")(0).ToString)
                    tbQtyWeek.Text = CInt(tbStart.Text) - CInt(tbEnd.Text) + 1
                    btnGetDt.Visible = True
                End If
                If ViewState("Sender") = "btnEnd" Then
                    BindToText(tbEnd, Session("Result")(0).ToString)
                    tbQtyWeek.Text = CInt(tbEnd.Text) - CInt(tbStart.Text) + 1
                End If

                If ViewState("Sender") = "btnMachine" Then
                    BindToText(tbMachine, Session("Result")(0).ToString)
                    BindToText(tbMachineName, Session("Result")(1).ToString)
                End If
                If ViewState("Sender") = "btnEquipment" Then
                    BindToText(tbEquip, Session("Result")(0).ToString)
                    BindToText(tbEquipName, Session("Result")(1).ToString)
                    BindToText(tbUnitE, Session("Result")(2).ToString)
                End If
                If ViewState("Sender") = "btnMaterial" Then
                    BindToText(tbMaterial, Session("Result")(0).ToString)
                    BindToText(tbMaterialName, Session("Result")(1).ToString)
                    BindToText(tbUnitM, Session("Result")(2).ToString)
                End If


                If ViewState("Sender") = "btnGetDt" Then
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim Divisi, Type As String
                    For Each drResult In Session("Result").Rows
                        Type = drResult("Type").ToString.Trim
                        Divisi = drResult("DivisiBlokCode").ToString
                        'Type, DivisiBlokCode, DivisiBlokName, PlantPeriodCode, PlantPeriodName, StartDate, EndDate, Qty, Capacity, , MaxCap
                        ExistRow = ViewState("Dt").Select("DivisiBlok = " + QuotedStr(Divisi) + " AND Type = " + QuotedStr(Type))

                        If ExistRow.Count = 0 Then
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            If ddlReffType.SelectedValue = "Schedule" Then
                                dr("DivisiBlok") = drResult("DivisiBlokCode").ToString
                                dr("DivisiBlokName") = drResult("DivisiBlokName").ToString
                                dr("StatusTanam") = drResult("PlantPeriodCode").ToString
                                dr("StatusTanamName") = drResult("PlantPeriodName").ToString
                                dr("Type") = drResult("Type").ToString
                                dr("StartDate") = drResult("StartDate")
                                dr("EndDate") = drResult("EndDate")
                                dr("Percentage") = 100
                                dr("Qty") = FormatFloat(drResult("Qty").ToString, ViewState("DigitQty"))
                                dr("QtyTotal") = FormatFloat(drResult("Qty").ToString, ViewState("DigitQty"))
                                dr("NormaHK") = FormatFloat(drResult("Capacity").ToString, ViewState("DigitQty"))

                                If (dr("NormaHK") > 0) And (tbPerson.Text > 0) Then
                                    dr("WorkDay") = (drResult("Qty") / dr("NormaHK") / tbPerson.Text)
                                    '   GetEndDate()
                                    dr("EndDate") = Date.FromOADate(dr("StartDate").ToOADate() + CInt(dr("WorkDay")))
                                Else
                                    dr("WorkDay") = 0
                                End If

                                If (dr("NormaHK") > 0) And (dr("Qty") > 0) Then
                                    dr("TargetHK") = CFloat(dr("Qty").ToString) / CFloat(dr("NormaHK").ToString)
                                Else : dr("TargetHK") = 0
                                End If
                                tbKontraktor.Enabled = False
                                btnKontraktor.Enabled = False


                                ' INNSERT KE GRID Material MEMALUI GET DETAIL=====================================================
                                Dim drResult3 As DataRow
                                Dim ExistRow2 As DataRow()
                                Dim dtUnitMT As DataTable
                                Dim drUnitMT As DataRow
                                Dim SQLString As String

                                'For Each drResult2 In Session("Result").Rows
                                ExistRow2 = ViewState("Dt4").Select("DivisiBlok = " + QuotedStr(drResult("DivisiBlokCode").ToString) + " AND Type = " + QuotedStr(drResult("Type").ToString))
                                If ExistRow.Count = 0 Then
                                    SQLString = "EXEC S_PLWOPlanFindRMReff 'Schedule'," + QuotedStr(tbReffNo.Text) + _
                                         "," + QuotedStr(tbJob.Text) + _
                                         "," + QuotedStr(drResult("DivisiBlokCode").ToString) + "," + QuotedStr(tbRotasi.Text) + _
                                         "," + QuotedStr(drResult("Type").ToString) + "," + QuotedStr(drResult("PlantPeriodCode").ToString) + _
                                         "," + QuotedStr(drResult("Qty").ToString) + "," + QuotedStr(drResult("Capacity").ToString)
                                    'SQLString = "EXEC S_PLWOPlanFindRMReff 'Schedule','IAL/PTB/1910/0001','21172','194','1','Batch','2009','6000',0"
                                    dtUnitMT = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                                    For Each drResult3 In dtUnitMT.Rows
                                        If dtUnitMT.Rows.Count > 0 Then
                                            drUnitMT = dtUnitMT.Rows(0)
                                            Dim drMt As DataRow
                                            drMt = ViewState("Dt4").NewRow
                                            drMt("Type") = drResult("Type").ToString
                                            drMt("DivisiBlok") = drResult("DivisiBlokCode").ToString
                                            drMt("Material") = drUnitMT("Material_Code").ToString
                                            drMt("MaterialName") = drUnitMT("Material_Name").ToString
                                            drMt("Qty") = FormatFloat(drUnitMT("Qty").ToString, ViewState("DigitQty"))
                                            drMt("Unit") = drUnitMT("Unit").ToString
                                            drMt("QtyTotal") = drResult("Qty").ToString

                                            'lbStatus.Text = dr2("Material")
                                            'Exit Sub

                                            If drResult("Qty").ToString > 0 Then
                                                drMt("QtyDosis") = drUnitMT("Qty") / drResult("Qty").ToString
                                            Else
                                                drMt("QtyDosis") = 0
                                            End If

                                            If drResult("Capacity").ToString > 0 Then
                                                drMt("QtyPokok") = drUnitMT("Qty") / drResult("Capacity").ToString
                                            Else : drMt("QtyPokok") = 0
                                                drMt("AltQty") = drUnitMT("Qty")
                                            End If
                                            ViewState("Dt4").Rows.Add(drMt)
                                        End If

                                    Next
                                    BindGridDt(ViewState("Dt4"), GridDt4)

                                End If
                                'Next
                                ''=======================================================================.>>>>


                            Else
                                '"type, DivisiBlokCode, DivisiBlokName, PlantPeriodCode, PlantPeriodName, Qty, Pkk, NormaKH, Area, MaxCap"
                                dr("Type") = drResult("Type").ToString
                                dr("DivisiBlok") = drResult("DivisiBlokCode").ToString
                                dr("DivisiBlokName") = drResult("DivisiBlokName").ToString
                                dr("StatusTanam") = drResult("PlantPeriodCode").ToString
                                dr("StatusTanamName") = drResult("PlantPeriodName").ToString
                                dr("StartDate") = Date.Today
                                dr("EndDate") = Date.Today
                                dr("Percentage") = 100
                                Dim dtUnit As DataTable
                                Dim drUnit As DataRow
                                dtUnit = SQLExecuteQuery("EXEC S_PLWOPlanFindDivBlock " + QuotedStr(ddlReffType.SelectedValue) + " , " + QuotedStr(tbReffNo.Text) + "," + QuotedStr(tbJob.Text) + "," + QuotedStr(drResult("Type").ToString) + ", " + QuotedStr(drResult("DivisiBlokCode").ToString) + ", " + QuotedStr(ddlDivisi.SelectedValue) + "," + tbRotasi.Text, ViewState("DBConnection")).Tables(0)
                                drUnit = dtUnit.Rows(0)

                                dr("Qty") = FormatFloat(drUnit("Qty").ToString, ViewState("DigitQty"))
                                dr("Pokok") = FormatFloat(drUnit("Pkk").ToString, ViewState("DigitQty"))
                                dr("QtyTotal") = FormatFloat(drUnit("Qty").ToString, ViewState("DigitQty"))
                                dr("NormaHK") = FormatFloat(drUnit("NormaHK").ToString, ViewState("DigitQty"))
                                If CFloat(drResult("Area").ToString) = 0 Then
                                    ViewState("SPH") = 0
                                Else
                                    ViewState("SPH") = CFloat(drUnit("Pkk").ToString) / CFloat(drResult("Area").ToString)
                                End If

                                If (dr("NormaHK") > 0) And (CInt(tbPerson.Text) > 0) Then
                                    dr("WorkDay") = (CFloat(drUnit("Qty")) / CFloat(dr("NormaHK")) / CInt(tbPerson.Text))
                                    'GetEndDate()
                                    dr("EndDate") = Date.FromOADate(dr("StartDate").ToOADate() + CInt(dr("WorkDay")))
                                Else
                                    dr("WorkDay") = 0
                                End If

                                If (dr("NormaHK") > 0) And (dr("Qty") > 0) Then
                                    dr("TargetHK") = CFloat(dr("Qty").ToString) / CFloat(dr("NormaHK").ToString)
                                Else
                                    dr("TargetHK") = 0
                                End If

                            End If
                            GetMachine(tbJob.Text, tbReffNo.Text, dr("DivisiBlok").ToString, drResult("Type").ToString)
                            'GetMaterial(ddlReffType.SelectedValue, tbReffNo.Text, tbJob.Text, dr("DivisiBlok").ToString, tbRotasi.Text, drResult("Type").ToString, drResult("PlantPeriodCode").ToString, dr("Qty"), dr("Pokok"))
                            ViewState("Dt").Rows.Add(dr)
                        End If

                    Next
                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt")) > 0
                    'Session("ResultSame") = Nothing
                End If

                If ViewState("Sender") = "btnOut" Then
                    BtnAdd_Click(Nothing, Nothing)
                    Dim drResult As DataRow
                    Dim ExistRow As DataRow()
                    Dim FirstTime As Boolean = True
                    Dim Divisi, Type, Rotasi, JobPlan As String
                    For Each drResult In Session("Result").Rows

                        Type = drResult("Type").ToString.Trim
                        Divisi = drResult("DivisiBlok").ToString
                        Rotasi = drResult("Rotasi").ToString
                        JobPlan = drResult("JobPlant").ToString

                        'insert
                        ExistRow = ViewState("Dt").Select("DivisiBlok = " + QuotedStr(Divisi) + " AND Type = " + QuotedStr(Type))
                        If ExistRow.Count = 0 Then
                            If FirstTime Then
                                'Type = drResult("Reff_Type").ToString.Trim
                                'Divisi = drResult("DivisiBlok").ToString
                                BindToText(tbReffNo, drResult("TransNmbr").ToString)
                                BindToText(tbRotasi, drResult("Rotasi").ToString)
                                BindToText(tbJob, drResult("JobPlant").ToString)
                                BindToText(tbJobName, drResult("JobPlant_Name").ToString)
                                BindToText(tbUnit, drResult("Unit").ToString)
                                ddlReffType.SelectedValue = "Schedule"
                                tbJob_TextChanged(Nothing, Nothing)
                                BindToDropList(ddlWorkBy, drResult("Team").ToString)
                                BindToDropList(ddlDivisi, drResult("Division").ToString)
                                'BindToText(tbfgHome, drResult("FgHome").ToString)
                                tbKontraktor.Enabled = False
                                btnKontraktor.Enabled = False
                            End If

                            If CekExistData(ViewState("Dt"), "DivisiBlok", drResult("DivisiBlok")) = False Then
                                Dim dr As DataRow
                                dr = ViewState("Dt").NewRow
                                'If ddlReffType.SelectedValue = "Schedule" Then
                                dr("DivisiBlok") = drResult("DivisiBlok").ToString
                                dr("DivisiBlokName") = drResult("DivisiBlok_Name").ToString
                                dr("StatusTanam") = drResult("PlantPeriodCode").ToString
                                dr("StatusTanamName") = drResult("PlantPeriodName").ToString
                                dr("Type") = drResult("Type").ToString
                                dr("StartDate") = drResult("StartDate")
                                dr("EndDate") = drResult("EndDate")
                                dr("Percentage") = 100
                                dr("Qty") = FormatFloat(drResult("Qty").ToString, ViewState("DigitQty"))
                                dr("QtyTotal") = FormatFloat(drResult("Qty").ToString, ViewState("DigitQty"))
                                dr("NormaHK") = FormatFloat(drResult("Capacity").ToString, ViewState("DigitQty"))

                                If (dr("NormaHK") > 0) And (tbPerson.Text > 0) Then
                                    dr("WorkDay") = (drResult("Qty") / dr("NormaHK") / tbPerson.Text)
                                    '   GetEndDate()
                                    dr("EndDate") = Date.FromOADate(dr("StartDate").ToOADate() + CInt(dr("WorkDay")))
                                Else
                                    dr("WorkDay") = 0
                                End If

                                If (dr("NormaHK") > 0) And (dr("Qty") > 0) Then
                                    dr("TargetHK") = CFloat(dr("Qty").ToString) / CFloat(dr("NormaHK").ToString)
                                Else : dr("TargetHK") = 0
                                End If
                                GetMachine(tbJob.Text, tbReffNo.Text, drResult("DivisiBlok").ToString, drResult("Type").ToString)
                                'GetMaterial(ddlReffType.SelectedValue = "Schedule", drResult("TransNmbr").ToString, drResult("JobPlant").ToString, drResult("DivisiBlok").ToString, drResult("Rotasi").ToString, drResult("Type").ToString, drResult("PlantPeriodCode").ToString, drResult("Qty").ToString, drResult("Capacity").ToString)
                                ViewState("Dt").Rows.Add(dr)
                            End If
                            FirstTime = False
                        End If
                        ' INNERT KE GRID MATERIAL ==============================================>>>>>
                        Dim drResult2, drResult3 As DataRow
                        Dim ExistRow2 As DataRow()
                        Dim dtUnit As DataTable
                        Dim drUnit, drow2 As DataRow
                        Dim SQLString As String

                        For Each drow2 In ViewState("Dt4").rows
                            drow2.Delete()
                        Next

                        For Each drResult2 In Session("Result").Rows
                            ExistRow2 = ViewState("Dt4").Select("DivisiBlok = " + QuotedStr(drResult("DivisiBlok").ToString) + " AND Type = " + QuotedStr(drResult("Type").ToString))
                            If ExistRow.Count = 0 Then
                                SQLString = "EXEC S_PLWOPlanFindRMReff 'Schedule'," + QuotedStr(drResult("TransNmbr").ToString) + _
                                "," + QuotedStr(drResult("JobPlant").ToString) + _
                                "," + QuotedStr(drResult("DivisiBlok").ToString) + "," + QuotedStr(drResult("Rotasi").ToString) + _
                                "," + QuotedStr(drResult("Type").ToString) + "," + QuotedStr(drResult("PlantPeriodCode").ToString) + _
                                "," + QuotedStr(drResult("Qty").ToString) + "," + QuotedStr(drResult("Capacity").ToString)
                                'SQLString = "EXEC S_PLWOPlanFindRMReff 'Schedule','IAL/PTB/1910/0001','21172','194','1','Batch','2009','6000',0"
                                dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)

                                For Each drResult3 In dtUnit.Rows
                                    If dtUnit.Rows.Count > 0 Then
                                        drUnit = dtUnit.Rows(0)
                                        Dim dr2 As DataRow
                                        dr2 = ViewState("Dt4").NewRow
                                        dr2("Type") = drResult("Type").ToString
                                        dr2("DivisiBlok") = drResult("DivisiBlok").ToString
                                        dr2("Material") = drUnit("Material_Code").ToString
                                        dr2("MaterialName") = drUnit("Material_Name").ToString
                                        dr2("Qty") = FormatFloat(drUnit("Qty").ToString, ViewState("DigitQty"))
                                        dr2("Unit") = drUnit("Unit").ToString
                                        dr2("QtyTotal") = drResult("Qty").ToString

                                        'lbStatus.Text = dr2("Material")
                                        'Exit Sub

                                        If drResult("Qty").ToString > 0 Then
                                            dr2("QtyDosis") = dr2("Qty") / drResult("Qty").ToString
                                        Else
                                            dr2("QtyDosis") = 0
                                        End If

                                        If drResult("Capacity").ToString > 0 Then
                                            dr2("QtyPokok") = dr2("Qty") / drResult("Capacity").ToString
                                        Else : dr2("QtyPokok") = 0
                                            dr2("AltQty") = dr2("Qty")
                                        End If
                                        ViewState("Dt4").Rows.Add(dr2)
                                    End If

                                Next
                                BindGridDt(ViewState("Dt4"), GridDt4)
                                EnableHd(GetCountRecord(ViewState("Dt4")) = 0)
                                GridDt4.Columns(1).Visible = GetCountRecord(ViewState("Dt4")) > 0

                            End If
                        Next
                        '=======================================================================.>>>>
                    Next

                    BindGridDt(ViewState("Dt"), GridDt)
                    EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt")) > 0

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
            'FillCombo(ddlDivisi, "EXEC S_PLWOPlanGetDivisionBLock", True, "DivisiBlokCode", "DivisiBlokName", ViewState("DBConnection"))
            FillCombo(ddlDivisi, "SELECT DivisionCode,DivisionName FROM V_MsDivisi WHERE FgBatch ='Y'", True, "DivisionCode", "DivisionName", ViewState("DBConnection").ToString)
            FillCombo(ddlWorkBy, "SELECT * FROM MsTeam Where DivType = 'Division' And Division IN ('194','191')", True, "TeamCode", "TeamName", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("SortExpressionOut") = Nothing
            ViewState("SortExpressionUse") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            tbQtyBlok.Attributes.Add("OnKeyDown", "return PressNumeric()")
            tbQtyBlok.Attributes.Add("OnBlur", "setformatdt()")
            tbQtyTotal.Attributes.Add("ReadOnly", "True")
            'tbQtyTotal.Attributes.Add("OnBlur", "setformatdt()")
            tbQtyE.Attributes.Add("OnKeyDown", "return PressNumeric()")
            tbQtyWeek.Attributes.Add("OnKeyDown", "return PressNumeric()")
            tbQtyWeek.Attributes.Add("OnBlur", "setformatdt()")
            tbPerson.Attributes.Add("OnKeyDown", "return PressNumeric()")
            tbRotasi.Attributes.Add("OnKeyDown", "return PressNumeric()")


            If ddlReffType.SelectedValue = "Schedule" Then
                tbKontraktor.Enabled = True
                btnKontraktor.Enabled = True
                tbCIP.Enabled = False
                btnCIP.Enabled = False
                tbJob.Enabled = False
                btnJob.Enabled = False
            Else
                tbKontraktor.Enabled = True
                btnKontraktor.Enabled = True
                tbCIP.Enabled = True
                btnCIP.Enabled = True
                tbJob.Enabled = True
                btnJob.Enabled = True
            End If


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
            SQLString = GetStringHd
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
        Return "SELECT * From V_PLWOPlanDt WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLWOPlanMachine WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLWOPlanEquip WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt4(ByVal Nmbr As String) As String
        Return "SELECT * From V_PLWOPlanRM WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                    Result = ExecSPCommandGo(ActionValue, "S_PLWOPlan", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            'btnGetDt.Visible = State

        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt3(ByVal State As Boolean)
        Try
            tbEquip.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Dt3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableDt4(ByVal State As Boolean)
        Try
            tbMachine.Enabled = State
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
            Drow = ViewState("Dt2").Select("Type+'|'+DivisiBlok=" + QuotedStr(LblTypeM.Text + "|" + LblBatchM.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "ViewA"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If
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
            Drow = ViewState("Dt3").Select("Type+'|'+DivisiBlok=" + QuotedStr(LblTypeE.Text + "|" + LblBatchE.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt3)
                GridDt3.Columns(0).Visible = Not ViewState("StateHd") = "ViewE"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt3").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt3.DataSource = DtTemp
                GridDt3.DataBind()
                GridDt3.Columns(0).Visible = False
            End If
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
            Drow = ViewState("Dt4").Select("Type+'|'+DivisiBlok=" + QuotedStr(LblTypeMT.Text + "|" + LblBatchMT.Text))
            If Drow.Length > 0 Then
                BindGridDt(Drow.CopyToDataTable, GridDt4)
                GridDt4.Columns(0).Visible = Not ViewState("StateHd") = "ViewM"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt4").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt4.DataSource = DtTemp
                GridDt4.DataBind()
                GridDt4.Columns(0).Visible = False
            End If
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
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlReffType.SelectedValue = "Other"
            tbReffNo.Text = ""
            tbRotasi.Text = "0"
            ddlWorkBy.SelectedValue = ""
            tbPerson.Text = "1"
            tbJob.Text = ""
            tbJobName.Text = ""
            tbCIP.Text = ""
            tbCIPName.Text = ""
            tbKontraktor.Text = ""
            tbKontraktorName.Text = ""
            tbQtyBlok.Text = "0"
            tbQty.Text = "0"
            tbStart.Text = ""
            tbEnd.Text = ""
            tbQtyWeek.Text = "0"
            tbRemark.Text = ""
            'Dim Division As String
            'Division = SQLExecuteScalar("EXEC S_GetPlantDivisionAll " + QuotedStr(ViewState("UserId")), ViewState("DBConnection"))
            'ddlDivisi.SelectedValue = "0"
            ddlDivisi_TextChanged(Nothing, Nothing)
            ddlReffType_TextChanged(Nothing, Nothing)
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            tbBatch.Text = ""
            tbBatchName.Text = ""
            ddlType.SelectedValue = "Batch"
            tbStatusTanam.Text = ""
            tbPercentage.Text = ""
            tbQtyWO.Text = "0"
            tbQtyCapacity.Text = "0"
            tbQtyTarget.Text = "0"
            tbQtyWorkDay.Text = "0"
            tbRemarkDt.Text = ""
            tbPercentage.Text = "100"
            tbTotal.Text = "0"
            tbStartDate.SelectedDate = ViewState("ServerDate")
            tbEndDate.SelectedDate = ViewState("ServerDate")
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            tbMachine.Text = ""
            tbMachineName.Text = ""
            tbQtyDuration.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt3()
        Try
            tbEquip.Text = ""
            tbEquipName.Text = ""
            tbQtyE.Text = "0"
            tbUnitE.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt4()
        Try
            tbMaterial.Text = ""
            tbMaterialName.Text = ""
            tbQtyDosis.Text = "0"
            tbQtyPokok.Text = "0"
            tbQtyTotal.Text = "0"
            tbQtyMaterial.Text = "0"
            tbUnitM.Text = ""

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
            If tbJob.Text = "" Then
                lbStatus.Text = MessageDlg("Job must have value")
                btnJob.Focus()
                Return False
            End If
            If LTrim(tbQtyWeek.Text) = 0 Then
                lbStatus.Text = MessageDlg("Qty Week must have value")
                btnStart.Focus()
                Return False
            End If
            If ddlWorkBy.SelectedValue = "" Then
                lbStatus.Text = MessageDlg("Work By must have value")
                ddlWorkBy.Focus()
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
                If Dr("Type").ToString = "" Then
                    lbStatus.Text = MessageDlg("Type Must Have Value")
                    Return False
                End If
                If TrimStr(Dr("DivisiBlok").ToString) = "" Then
                    lbStatus.Text = MessageDlg("Divisi / Batch Must Have Value")
                    Return False
                End If
                If LTrim(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty WO Must Have Value")
                    Return False
                End If
                If CFloat(Dr("NormaHK").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Kapasitas (HK) Must Have Value")
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
                'If Dr("Warehouse").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Warehouse Must Have Value")
                '    Return False
                'End If
                'If Dr("Subled").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Subled Must Have Value")
                '    Return False
                'End If

            Else
                'If ddlStatusTanam.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Warehouse must have value")
                '    ddlStatusTanam.Focus()
                '    Return False
                'End If
                'If tbSubLed.Text.Trim = "" And tbFgSubLed.Text.Trim <> "N" Then
                '    lbStatus.Text = MessageDlg("SubLed must have value")
                '    tbSubLed.Focus()
                '    Return False
                'End If

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
                'If Dr("Machine").ToString = "" Then
                '    lbStatus.Text = MessageDlg("Machine Must Have Value")
                '    Return False
                'End If
                ''If Dr("StartTime").ToString = "00:00" Then
                ''    lbStatus.Text = MessageDlg("Start Time Must Have Value")
                ''    Return False
                ''End If
                'If CFloat(Dr("Duration").ToString) <= 0 Then
                '    lbStatus.Text = MessageDlg("Duration Must Have Value")
                '    Return False
                'End If


            Else
                'If tbMachineNameDt4.Text = "" Then
                '    lbStatus.Text = MessageDlg("Machine must have value")
                '    tbMachineCodeDt4.Focus()
                '    Return False
                'End If
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
        Try

            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRotasi, Dt.Rows(0)("MonthNo").ToString)
            BindToDropList(ddlDivisi, Dt.Rows(0)("Divisi").ToString)
            'FillCombo(ddlWorkBy, "EXEC S_GetTeamWO " + QuotedStr(ddlDivisi.SelectedValue) + ",''", True, "TeamCode", "TeamName", ViewState("DBConnection"))
            FillCombo(ddlWorkBy, "SELECT * FROM MsTeam Where DivType = 'Division' And Division IN ('194','191')", True, "TeamCode", "TeamName", ViewState("DBConnection"))
            BindToDropList(ddlWorkBy, Dt.Rows(0)("WorkBy").ToString)
            BindToDropList(ddlReffType, Dt.Rows(0)("ReffType").ToString)
            BindToText(tbReffNo, Dt.Rows(0)("Reference").ToString)
            BindToText(tbPerson, Dt.Rows(0)("Person").ToString)
            BindToText(tbJob, Dt.Rows(0)("JobPlant").ToString)
            BindToText(tbJobName, Dt.Rows(0)("JobPlantName").ToString)
            BindToText(tbCIP, Dt.Rows(0)("CIP").ToString)
            BindToText(tbCIPName, Dt.Rows(0)("CIPName").ToString)
            BindToText(tbKontraktor, Dt.Rows(0)("Supplier").ToString)
            BindToText(tbKontraktorName, Dt.Rows(0)("SupplierName").ToString)
            BindToText(tbQtyBlok, Dt.Rows(0)("QtyBlok").ToString)
            BindToText(tbQty, FormatNumber(Dt.Rows(0)("Qty").ToString), 2)
            BindToText(tbUnit, Dt.Rows(0)("Unit").ToString)
            BindToText(tbStart, Dt.Rows(0)("EstStartWeek").ToString)
            BindToText(tbEnd, Dt.Rows(0)("EstEndWeek").ToString)
            BindToText(tbQtyWeek, Dt.Rows(0)("QtyWeek").ToString)
            BindToText(tbRemark, Dt.Rows(0)("Remark").ToString)

        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String, ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("Type = " + QuotedStr(RRNo) + " AND DivisiBlok = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then

                BindToDropList(ddlType, Dr(0)("Type").ToString)
                BindToText(tbBatch, Dr(0)("DivisiBlok").ToString)
                BindToText(tbBatchName, Dr(0)("DivisiBlokName").ToString)
                BindToText(tbStatusTanam, Dr(0)("statusTanamName").ToString)
                BindToText(tbPercentage, CFloat(Dr(0)("Percentage").ToString))
                BindToText(tbTotal, CFloat(Dr(0)("QtyTotal").ToString))
                BindToText(tbQtyWO, CFloat(Dr(0)("Qty").ToString))
                BindToText(tbQtyCapacity, CFloat(Dr(0)("NormaHK").ToString))
                BindToText(tbQtyTarget, CFloat(Dr(0)("TargetHK").ToString))
                BindToText(tbQtyWorkDay, Dr(0)("WorkDay").ToString)
                BindToDate(tbStartDate, Dr(0)("StartDate").ToString)
                BindToDate(tbEndDate, Dr(0)("EndDate").ToString)

            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("Machine = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToText(tbMachine, Dr(0)("Machine").ToString)
                BindToText(tbMachineName, Dr(0)("MachineName").ToString)
                BindToText(tbQtyDuration, Dr(0)("EstHour").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt3(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("Equipment = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then

                BindToText(tbEquip, Dr(0)("Equipment").ToString)
                BindToText(tbEquipName, Dr(0)("EquipmentName").ToString)
                BindToText(tbQtyE, Dr(0)("Qty").ToString)
                BindToText(tbUnitE, Dr(0)("Unit").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt4(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt4").select("Material = " + ItemNo)
            If Dr.Length > 0 Then

                BindToText(tbMaterial, Dr(0)("Material").ToString)
                BindToText(tbMaterialName, Dr(0)("MaterialName").ToString)
                BindToText(tbQtyMaterial, Dr(0)("Qty").ToString)
                BindToText(tbUnitM, Dr(0)("Unit").ToString)
                BindToText(tbQtyTotal, Dr(0)("QtyTotal").ToString)
                BindToText(tbQtyDosis, FormatNumber(Dr(0)("QtyDosis").ToString), 2)
                BindToText(tbQtyPokok, FormatNumber(Dr(0)("QtyPokok").ToString), 2)

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
                Row = ViewState("Dt").Select("Type+'|'+DivisiBlok = " + QuotedStr(ViewState("DtValue")))(0)
                Row.BeginEdit()
                Row("DivisiBlok") = tbBatch.Text
                Row("Type") = ddlType.SelectedValue
                Row("DivisiBlokName") = tbBatchName.Text
                Row("StatusTanamName") = tbStatusTanam.Text
                Row("Percentage") = tbPercentage.Text
                Row("QtyTotal") = FormatFloat(tbTotal.Text, ViewState("DigitQty"))
                Row("Qty") = FormatFloat(tbQtyWO.Text, ViewState("DigitQty"))
                Row("NormaHK") = FormatFloat(tbQtyCapacity.Text, ViewState("DigitQty"))
                Row("TargetHK") = FormatFloat(tbQtyTarget.Text, ViewState("DigitQty"))
                Row("WorkDay") = FormatFloat(tbQtyWorkDay.Text, ViewState("DigitQty"))
                Row("StartDate") = tbStartDate.SelectedDate
                Row("EndDate") = tbEndDate.SelectedDate
                Row.EndEdit()
            Else
                If CekExistData(ViewState("Dt"), "Type,DivisiBlok", ddlType.Text + "|" + TrimStr(tbBatch.Text)) = True Then
                    lbStatus.Text = "Type " + ddlType.SelectedValue + " Batch " + tbBatch.Text + " has been already exist"
                    Exit Sub
                End If
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("DivisiBlok") = tbBatch.Text
                dr("Type") = ddlType.SelectedValue
                dr("DivisiBlokName") = tbBatchName.Text
                dr("StatusTanamName") = tbStatusTanam.Text
                dr("Percentage") = tbPercentage.Text
                dr("QtyTotal") = tbQtyTotal.Text
                dr("Qty") = tbQtyWO.Text
                dr("NormaHK") = tbQtyCapacity.Text
                dr("TargetHK") = tbQtyTarget.Text
                dr("WorkDay") = tbQtyWorkDay.Text
                dr("StartDate") = tbStartDate.Text
                dr("EndDate") = tbEndDate.Text
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
                Row = ViewState("Dt2").Select("Machine = " + QuotedStr(ViewState("Dt2Value")))(0)
                Row.BeginEdit()
                Row("Machine") = tbMachine.Text
                Row("MachineName") = tbMachineName.Text
                Row("EstHour") = tbQtyDuration.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt2").NewRow
                dr("Type") = LblTypeM.Text
                dr("DivisiBlok") = LblBatchM.Text
                dr("Machine") = tbMachine.Text
                dr("Machine") = tbMachine.Text
                dr("Machine") = tbMachine.Text
                dr("MachineName") = tbMachineName.Text
                dr("EstHour") = tbQtyDuration.Text

                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            'EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0 = 0)
            BindGridDt(ViewState("Dt2"), GridDt2)
            'Dim drow As DataRow()
            'drow = ViewState("Dt2").Select("Type+'|'+DivisiBlok=" + QuotedStr(lbWODt2.Text))
            'If drow.Length > 0 Then
            '    BindGridDt(drow.CopyToDataTable, GridDt2)
            '    GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            'Else
            '    Dim DtTemp As DataTable
            '    DtTemp = ViewState("Dt2").Clone
            '    DtTemp.Rows.Add(DtTemp.NewRow())
            '    GridDt2.DataSource = DtTemp
            '    GridDt2.DataBind()
            '    GridDt2.Columns(0).Visible = False
            'End If
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
                tbCode.Text = GetAutoNmbr("WPL", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PLWOPlanHd (TransNmbr, TransDate, STATUS, FgBorongan, FgBatch, Divisi, " + _
                "WorkBy, ReffType, Reference, MonthNo, JobPlant, QtyBlok, Qty, Unit, EstStartWeek, EstEndWeek, QtyWeek, Remark, " + _
                "CIP, Supplier, Person, UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', 'N', 'N'," + QuotedStr(ddlDivisi.SelectedValue) + "," + _
                QuotedStr(ddlWorkBy.SelectedValue) + "," + QuotedStr(ddlReffType.SelectedValue) + "," + QuotedStr(tbReffNo.Text) + "," + _
                QuotedStr(tbRotasi.Text) + "," + QuotedStr(tbJob.Text) + "," + tbQtyBlok.Text.Replace(", ", "") + "," + _
                tbQty.Text.Replace(", ", "") + "," + QuotedStr(tbUnit.Text) + "," + _
                QuotedStr(tbStart.Text) + "," + QuotedStr(tbEnd.Text) + "," + tbQtyWeek.Text.Replace(", ", "") + "," + _
                QuotedStr(tbRemark.Text) + "," + QuotedStr(tbCIP.Text) + "," + QuotedStr(tbKontraktor.Text) + "," + _
                QuotedStr(tbPerson.Text) + "," + QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PLWOPlanHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PLWOPlanHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", WorkBy = " + QuotedStr(ddlWorkBy.SelectedValue) + ", ReffType = " + QuotedStr(ddlReffType.SelectedValue) + _
                ", Person = " + QuotedStr(tbPerson.Text) + ", Reference = " + QuotedStr(tbReffNo.Text) + _
                ", CIP = " + QuotedStr(tbCIP.Text) + ", Supplier = " + QuotedStr(tbKontraktor.Text) + _
                ", MonthNo = " + QuotedStr(tbRotasi.Text) + ", JobPlant = " + QuotedStr(tbJob.Text) + ", QtyBlok=" + tbQtyBlok.Text.Replace(", ", "") + _
                ", Qty= " + QuotedStr(tbQty.Text.Replace(",", "").Replace(".00", "")) + ", Unit =" + QuotedStr(tbUnit.Text) + _
                ", EstStartWeek = " + QuotedStr(tbStart.Text) + ", EstEndWeek = " + QuotedStr(tbEnd.Text) + ", QtyWeek=" + tbQtyWeek.Text.Replace(", ", "") + _
                ", Remark = " + QuotedStr(tbRemark.Text) + ", DatePrep = GetDate()" + _
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
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, Type, DivisiBlok, StartDate, EndDate, Qty, QtyResult, WorkDay, NormaHK, Percentage, QtyTotal, StatusTanam FROM PLWOPlanDt WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            'da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            'da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim param As SqlParameter
            ' Create the UpdateCommand.
            Dim Update_Command = New SqlCommand( _
                    "UPDATE PLWOPlanDt SET Type = @Type, DivisiBlok = @DivisiBlok, StartDate = @StartDate, EndDate = @EndDate, QtyResult = @QtyResult, WorkDay = @WorkDay, NormaHK = @NormaHK, Percentage = @Percentage, QtyTotal = @QtyTotal,  " + _
                    " StatusTanam = @StatusTanam WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Type = @OldType AND DivisiBlok = @OldDivisiBlok ", con)
            ' Define output parameters.
            Update_Command.Parameters.Add("@Type", SqlDbType.VarChar, 8, "Type")
            Update_Command.Parameters.Add("@DivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            Update_Command.Parameters.Add("@StartDate", SqlDbType.DateTime, 8, "StartDate")
            Update_Command.Parameters.Add("@EndDate", SqlDbType.DateTime, 8, "EndDate")
            Update_Command.Parameters.Add("@QtyResult", SqlDbType.Float, 18, "QtyResult")
            Update_Command.Parameters.Add("@WorkDay", SqlDbType.Float, 18, "WorkDay")
            Update_Command.Parameters.Add("@NormaHK", SqlDbType.Float, 18, "NormaHK")
            Update_Command.Parameters.Add("@Percentage", SqlDbType.Float, 18, "Percentage")
            Update_Command.Parameters.Add("@QtyTotal", SqlDbType.Float, 18, "QtyTotal")
            Update_Command.Parameters.Add("@StatusTanam", SqlDbType.VarChar, 60, "StatusTanam")


            ' Define intput (WHERE) parameters.
            param = Update_Command.Parameters.Add("@OldType", SqlDbType.VarChar, 8, "Type")
            param.SourceVersion = DataRowVersion.Original
            param = Update_Command.Parameters.Add("@OldDivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            param.SourceVersion = DataRowVersion.Original
            ' Attach the update command to the DataAdapter.
            da.UpdateCommand = Update_Command

            ' Create the DeleteCommand.
            Dim Delete_Command = New SqlCommand( _
                "DELETE FROM PLWOPlanDt WHERE TransNmbr = '" & ViewState("TransNmbr") & "' AND Type = @Type AND DivisiBlok = @DivisiBlok ", con)
            ' Add the parameters for the DeleteCommand.
            param = Delete_Command.Parameters.Add("@DivisiBlok", SqlDbType.VarChar, 20, "DivisiBlok")
            param.SourceVersion = DataRowVersion.Original
            param = Delete_Command.Parameters.Add("@Type", SqlDbType.VarChar, 8, "Type")
            param.SourceVersion = DataRowVersion.Original
            da.DeleteCommand = Delete_Command

            Dim Dt As New DataTable("PLWOPlanDt")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, DivisiBlok, Type, Machine, EstHour FROM PLWOplanMachine WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PLWOplanMachine")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, DivisiBlok, Type, Equipment, Qty, Unit FROM PLWOPlanEquip WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt3 As New DataTable("PLWOPlanEquip")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3

            'save dt4
            cmdSql = New SqlCommand("SELECT TransNmbr, DivisiBlok, Type, Material, Qty, Unit, Remark, QtyIssue, QtyTotal, AltMaterial, AltQty, AltUnit, AltUserId, AltUserDate FROM PLWOPlanRM WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt4 As New DataTable("PLWOPlanRM")

            Dt4 = ViewState("Dt4")
            da.Update(Dt4)
            Dt4.AcceptChanges()
            ViewState("Dt4") = Dt4



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
            GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt2")) > 0
            MovePanel(PnlHd, pnlInput)
            EnableHd(True)
            btnAddDt.Visible = False
            btnAddDt2.Visible = GetCountRecord(ViewState("Dt2")) > 0
            btnAddDt3.Visible = GetCountRecord(ViewState("Dt3")) > 0
            btnAdddt4.Visible = GetCountRecord(ViewState("Dt4")) > 0
            btnAddDtKe2.Visible = False
            btnAddDt2ke2.Visible = GetCountRecord(ViewState("Dt2")) > 0
            btnAddDt3ke2.Visible = GetCountRecord(ViewState("Dt3")) > 0
            btnAddDt4ke2.Visible = GetCountRecord(ViewState("Dt4")) > 0
            tbCode.Focus()
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
            tbMaterial.Focus()
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
            tbBatch.Focus()
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
                    btnGetDt.Visible = False
                    ModifyInput2(False, pnlInput, pnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    ModifyInput2(False, pnlInput, PnlDt4, GridDt4)

                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                    'GridDt.Columns(1).Visible = True
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
                        btnAddDt.Visible = False
                        btnAddDtKe2.Visible = False
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
                        Session("SelectCommand") = "EXEC S_PLWOPlanForm2 ' ANd A.Transnmbr in (''" + GVR.Cells(2).Text + "'')'  " + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormWOPlanBatch.frx"
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
                    drow = ViewState("Dt2").Select("Type+'|'+DivisiBlok = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text))
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
                LblTypeM.Text = GVR.Cells(2).Text
                LblBatchM.Text = GVR.Cells(3).Text
                LblBatchNameM.Text = GVR.Cells(4).Text
                Dim drow As DataRow()
                If ViewState("Dt2") Is Nothing Then
                    BindDataDt2(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt2").Select("Type+'|'+DivisiBlok = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text))
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
                LblTypeE.Text = GVR.Cells(2).Text
                LblBatchE.Text = GVR.Cells(3).Text
                LblBatchNameE.Text = GVR.Cells(4).Text
                Dim drow As DataRow()
                If ViewState("Dt3") Is Nothing Then
                    BindDataDt3(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt3").Select("Type+'|'+DivisiBlok = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text))
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
                LblTypeMT.Text = GVR.Cells(2).Text
                LblBatchMT.Text = GVR.Cells(3).Text
                LblBatchNameMT.Text = GVR.Cells(4).Text

                Dim drow As DataRow()
                If ViewState("Dt4") Is Nothing Then
                    BindDataDt4(ViewState("TransNmbr"))
                Else
                    drow = ViewState("Dt4").Select("Type+'|'+DivisiBlok = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text))
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
                'BindGridDt(ViewState("Dt4"), GridDt4)
                'If ddlReffType.SelectedValue = "Schedule" Then
                '    btnGetData_Click(Nothing, Nothing)
                '    btnGetData.Visible = True
                'Else
                '    btnGetData.Visible = False
                'End If
                End If
        Catch ex As Exception
            lbStatus.Text = lbStatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub
    Dim TQtyWO As Double = 0

    Dim MaxItem As Integer = 0
    Protected Sub GridDt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridDt.RowDataBound
        Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "DivisiBlok")) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    MaxItem = MaxItem + 1
                    TQtyWO = GetTotalSum(ViewState("Dt"), "Qty")
                ElseIf e.Row.RowType = DataControlRowType.Footer Then
                End If
            End If
            tbQtyBlok.Text = MaxItem
            tbQty.Text = TQtyWO
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
            dr = ViewState("Dt").Select("Type = " + QuotedStr(GVR.Cells(2).Text) + " AND DivisiBlok = " + QuotedStr(GVR.Cells(3).Text))
            For Each r In dr
                r.Delete()
            Next

            For i = 0 To GetCountRecord(ViewState("Dt2")) - 1
                drt = ViewState("Dt2").Rows(i)
                If Not drt.RowState = DataRowState.Deleted Then
                    drt.Delete()
                End If
            Next

            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt4")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("Machine = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            'BindGridDt(ViewState("Dt2"), GridDt2)
            dr = ViewState("Dt2").Select("Machine = " + QuotedStr(GVR.Cells(1).Text))
            If dr.Length > 0 Then
                BindGridDt(dr.CopyToDataTable, GridDt2)
                GridDt2.Columns(0).Visible = Not ViewState("StateHd") = "View"
            Else
                Dim DtTemp As DataTable
                DtTemp = ViewState("Dt2").Clone
                DtTemp.Rows.Add(DtTemp.NewRow())
                GridDt2.DataSource = DtTemp
                GridDt2.DataBind()
                GridDt2.Columns(0).Visible = False
            End If

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
                lbStatus.Text = MessageDlg("Detail 'WO' must have at least 1 record")
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
            tbEquip.Focus()
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

            If ViewState("StateDt3") = "Edit" Then
                Dim Row As DataRow
                Row = ViewState("Dt3").Select("Equipment = " + QuotedStr(ViewState("Dt3Value")))(0)
                Row.BeginEdit()
                Row("Equipment") = tbEquip.Text
                Row("EquipmentName") = tbEquipName.Text
                Row("Qty") = tbQtyE.Text
                Row("Unit") = tbUnitE.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("Type") = LblTypeE.Text
                dr("DivisiBlok") = LblBatchE.Text
                dr("Equipment") = tbEquip.Text
                dr("EquipmentName") = tbEquipName.Text
                dr("Qty") = tbQtyE.Text
                dr("Unit") = tbUnitE.Text
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
                If Dr("Machine").ToString = "" Then
                    lbStatus.Text = MessageDlg("Machine Must Have Value")
                    Return False
                End If
                If Dr("MachineName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Material Must Have Value")
                    Return False
                End If
                If CFloat(Dr("Qty").ToString) <= 0 Then
                    lbStatus.Text = MessageDlg("Qty Must Have Value")
                    Return False
                End If
                If Dr("Unit").ToString = "" Then
                    lbStatus.Text = MessageDlg("Unit Must Have Value")
                    Return False
                End If
            Else

                'If tbtbEquipName.Text.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Material Must Have Value")
                '    tbtbEquipName.Focus()
                '    Return False
                'End If
                'If CFloat(tbQtyDt3.Text.Trim) <= 0 Then
                '    lbStatus.Text = MessageDlg("Qty Must Have Value")
                '    tbQtyDt3.Focus()
                '    Return False
                'End If
                'If ddlUnitDt3.SelectedValue.Trim = "" Then
                '    lbStatus.Text = MessageDlg("Unit Must Have Value")
                '    ddlUnitDt3.Focus()
                '    Return False
                'End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt3.Rows(e.RowIndex)
            dr = ViewState("Dt3").Select("Equipment = " + QuotedStr(GVR.Cells(1).Text))
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
            FillTextBoxDt3(GVR.Cells(1).Text)
            ViewState("Dt3Value") = GVR.Cells(1).Text
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJob.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Job_Code, Job_Name, Level_Plant, Job_No, Unit, FgCIP From V_MsJobPlant WHERE Level_Plant IN ('03','04') "
            ResultField = "Job_Code, Job_Name, Unit, FgCIP"
            ViewState("Sender") = "btnJob"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnJob_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnCIP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCIP.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT  CIPCode, CIPName, Estate from V_MsCIP " ' WHERE Car_No = " + QuotedStr(tbCarT.Text)
            ResultField = "CIPCode, CIPName"
            ViewState("Sender") = "btnCIP"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Search Cust Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnKontraktor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKontraktor.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT   Kontraktor_Code, Kontraktor_Name from V_MsKontraktor"
            ResultField = "Kontraktor_Code, Kontraktor_Name"
            ViewState("Sender") = "btnKontraktor"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnKontraktor_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT  Week_No, dbo.FormatDate(Start_Date) As Start_Date, dbo.FormatDate(End_Date) As End_Date from V_MsWeekNo WHERE Week_No >= dbo.GetWeekNo(" + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ")"
            ResultField = "Week_No"
            ViewState("Sender") = "btnStart"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnStart_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnEnd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnd.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Week_No, dbo.FormatDate(Start_Date) As Start_Date, dbo.FormatDate(End_Date) as End_Date from V_MsWeekNo WHERE Week_No >= dbo.GetWeekNo(" + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ")"
            ResultField = "Week_No"
            ViewState("Sender") = "btnEnd"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEnd_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub btnMachine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMachine.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT  Machine_Code, Machine_Name From V_MsMachine "
            ResultField = "Machine_Code, Machine_Name"
            ViewState("Sender") = "btnMachine"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMachine_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub tbMachine_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMachine.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select  Machine_Code, Machine_Name From V_MsMachine WHERE Machine_Code = " + QuotedStr(tbMachine.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMachine.Text = Dr("Machine_Code")
                tbMachineName.Text = Dr("Machine_Name")
                btnMachine.Focus()
            Else
                tbMachine.Text = ""
                tbMachineName.Text = ""
                tbMachine.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tbMachine_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnEquip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEquip.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Equipment, Equipment_Name, Unit from V_MsEquipment Where Type = 'Equipment'"
            ResultField = "Equipment, Equipment_Name, Unit"
            ViewState("Sender") = "btnEquipment"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnEquip_Click Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub tbEquip_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEquip.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select Equipment, Equipment_Name, Unit from V_MsEquipment Where Type = ''Equipment'' AND  Equipment = " + QuotedStr(tbEquip.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbEquip.Text = Dr("Equipment")
                tbEquipName.Text = Dr("Equipment_Name")
                tbUnitE.Text = Dr("Equipment_Name")
                btnEquip.Focus()
            Else
                tbEquip.Text = ""
                tbEquipName.Text = ""
                tbUnitE.Text = ""
                tbEquip.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("btnEquip_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnMaterial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaterial.Click
        Dim ResultField As String
        Try
            Session("filter") = "SELECT Material_Code, Material_Name, Unit from V_MsMaterial"
            ResultField = "Material_Code, Material_Name, Unit"
            ViewState("Sender") = "btnMaterial"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnMaterial_Click Error : " + ex.ToString
        End Try


    End Sub

    Protected Sub tbMaterial_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMaterial.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("select Material_Code, Material_Name, Unit from V_MsMaterial WHERE Material_Code = " + QuotedStr(tbMaterial.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbMaterial.Text = Dr("Material_Code")
                tbMaterialName.Text = Dr("Material_Name")
                tbUnitM.Text = Dr("Unit")
                btnMaterial.Focus()
            Else
                tbMaterial.Text = ""
                tbMaterialName.Text = ""
                tbUnitM.Text = ""
                tbMaterial.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbMaterial_TextChanged Error : " + ex.ToString)
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
                Row("Material") = tbMaterial.Text
                Row("MaterialName") = tbMaterialName.Text
                Row("Qty") = tbQtyMaterial.Text
                Row("Unit") = tbUnitM.Text
                Row("QtyTotal") = tbQtyTotal.Text
                Row("QtyDosis") = tbQtyDosis.Text
                Row("QtyPokok") = tbQtyPokok.Text
                Row("Remark") = tbRemarkDt.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt4").NewRow
                dr("Type") = LblTypeMT.Text
                dr("DivisiBlok") = LblBatchMT.Text
                dr("Material") = tbMaterial.Text
                dr("MaterialName") = tbMaterialName.Text
                dr("Qty") = tbQtyMaterial.Text
                dr("Unit") = tbUnitM.Text
                dr("QtyTotal") = tbQtyTotal.Text
                dr("QtyDosis") = tbQtyDosis.Text
                dr("QtyPokok") = tbQtyPokok.Text
                dr("Remark") = tbRemarkDt.Text
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

    Protected Sub GridDt4_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt4.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt4.Rows(e.RowIndex)
            dr = ViewState("Dt4").Select("Material = " + GVR.Cells(1).Text)
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

    'Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
    '    Dim index As Integer
    '    Try
    '        index = Int32.Parse(e.Item.Value)
    '        MultiView.ActiveViewIndex = index
    '        If MultiView.ActiveViewIndex = 1 Then
    '            FillCombo(ddlUseRollNo, "SELECT RollNo FROM V_PLWOPlanRollUse WHERE TransNmbr = " + QuotedStr(tbLotLHPNo.Text) + " AND WOItemNo = " + QuotedStr(ddlLotWOItemNo.SelectedValue), True, "RollNo", "RollNo", ViewState("DBConnection"))
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "Menu Item Click Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
    '    If (tbGenerateNoStart.Text.Trim = "") Or (tbGenerateNoEnd.Text.Trim = "") Or (tbGenerateSize.Text.Trim = "") Or (tbGenerateLength.Text.Trim = "") Or (tbGenerateWeight.Text.Trim = "") Or (tbGenerateGSM.Text.Trim = "") Or (tbGeneratePrefix.Text.Trim = "") Or (tbGeneretDigit.Text.Trim = "") Then
    '        lbStatus.Text = "Set Lot No must be complete"
    '        tbGenerateNoStart.Focus()
    '        Exit Sub
    '    End If
    '    If (CInt(tbGenerateNoStart.Text) = 0) Or (CInt(tbGenerateNoEnd.Text) = 0) Or (CFloat(tbGenerateSize.Text) = 0) Or (CFloat(tbGenerateLength.Text) = 0) Or (CFloat(tbGenerateWeight.Text) = 0) Or (CFloat(tbGenerateGSM.Text) = 0) Or (tbGeneratePrefix.Text.Trim = "") Or (CInt(tbGeneretDigit.Text) = 0) Then
    '        lbStatus.Text = "Set Lot No must be complete"
    '        tbGenerateNoStart.Focus()
    '        Exit Sub
    '    End If
    '    If CInt(tbGeneratePrefix.Text.Length) >= CInt(tbGeneretDigit.Text) Then
    '        lbStatus.Text = "Prefix can not greater than Digit"
    '        tbGeneratePrefix.Focus()
    '        Exit Sub
    '    End If
    '    'Dim SQLString As String
    '    'SQLString = "EXEC S_PLWOPlanGetNoLot " + QuotedStr(tbGeneratePrefix.Text) + ", " + tbGenerateNoStart.Text + ", " + tbGenerateNoEnd.Text + ", " + tbGeneretDigit.Text + ", " + QuotedStr(tbGenerateSufix.Text)

    '    'lbStatus.Text = SQLString
    '    'Exit Sub
    '    'If ddlUseRollNo.SelectedValue = "" Then
    '    '    lbStatus.Text = "Use roll No must have value"
    '    '    ddlUseRollNo.Focus()
    '    '    Exit Sub
    '    'End If
    '    BindDataSetNoLot(ddlUseRollNo.SelectedValue, ddlStatus.SelectedValue)
    'End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(2).Text, GVR.Cells(3).Text)
            ViewState("DtValue") = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text
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


    Protected Sub ddlDivisi_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivisi.TextChanged
        Try
            'FillCombo(ddlWorkBy, "EXEC S_GetTeamWO " + QuotedStr(ddlDivisi.SelectedValue) + ",''", True, "TeamCode", "TeamName", ViewState("DBConnection"))
            FillCombo(ddlWorkBy, "SELECT * FROM MsTeam Where DivType = 'Division' And Division IN ('194','191')", True, "TeamCode", "TeamName", ViewState("DBConnection"))
        Catch ex As Exception
            Throw New Exception("ddlDivisi_TextChanged Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnReff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReff.Click
        Dim ResultField As String
        Try
            'Session("filter") = "SELECT  TransNmbr, Team, Person, JobPlant, JobPlant_Name, Rotasi, StartDate, EndDate, Qty, Section, RK_Year, RK_Month, Supplier, SuppName From V_PLWOPlanGetReff where Division =" + QuotedStr(ddlDivisi.SelectedValue)
            'Session("filter") = "SELECT  TransNmbr, Team, JobPlant, JobPlant_Name, Rotasi, StartDate, EndDate, Qty, Section, RK_Year, RK_Month From V_PLWOPlanGetReff where Division =" + QuotedStr(ddlDivisi.SelectedValue)
            Session("filter") = "SELECT A.TransNmbr, A.JobPlant, A.JobPlant_Name, A.Rotasi, A.Team, A.Unit FROM V_PLWOPlanGetReff A LEFT OUTER JOIN PLWOPlanHd B ON A.TransNmbr = B.Reference And A.JobPlant = B.JobPlant And A.Rotasi = B.MonthNo WHere B.Status Is NULL and A.Reff_Type = 'Batch' And A.Division = " + QuotedStr(ddlDivisi.SelectedValue)
            ResultField = "TransNmbr, JobPlant, JobPlant_Name, Rotasi, Team, Unit"
            ViewState("Sender") = "btnReff"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnReff_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnGetDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDt.Click
        Dim ResultField As String
        Try
            If ddlReffType.SelectedValue = "Schedule" Then
                Session("filter") = "EXEC S_PLWOPlanReff " + QuotedStr(ddlReffType.SelectedValue) + ", ' A.TransNmbr = '" + QuotedStr(tbReffNo.Text) + "' And A.Division = '" + QuotedStr(ddlDivisi.SelectedValue) + "' And A.Rotasi = " + tbRotasi.Text + " And A.JobPlant = '" + QuotedStr(tbJob.Text) + "'' "
                'Session("filter") = "SELECT A.*, B.Status FROM V_PLWOPlanGetReff A LEFT OUTER JOIN PLWOPlanHd B ON A.TransNmbr = B.Reference And A.JobPlant = B.JobPlant And A.Rotasi = B.MonthNo  WHere B.Status Is NULL AND A.Reff_Type = 'Batch' AND A.TransNmbr = " + QuotedStr(tbReffNo.Text) + " And A.Division = " + QuotedStr(ddlDivisi.SelectedValue) + " And A.Rotasi = " + tbRotasi.Text + " And A.JobPlant = " + QuotedStr(tbJob.Text) + "  "
                ResultField = "Type, DivisiBlokCode, DivisiBlokName, PlantPeriodCode, PlantPeriodName, StartDate, EndDate, Qty, Area, MaxCap, Capacity"
            Else
                Session("filter") = "EXEC S_PLWOPlanReff 'Other', '  And Type=''Batch'' And Divisi =  '" + QuotedStr(ddlDivisi.SelectedValue) + "''"
                ResultField = "Type, DivisiBlokCode, DivisiBlokName, PlantPeriodCode, PlantPeriodName, Area"
            End If

            'lbStatus.Text = Session("filter")
            '            Exit Sub
            ViewState("Sender") = "btnGetDt"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btnGetDt Error : " + ex.ToString
        End Try


    End Sub

    Protected Sub ddlReffType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReffType.TextChanged
        Try

            If ddlReffType.SelectedValue = "Schedule" Then
                tbKontraktor.Enabled = True
                btnKontraktor.Enabled = True
                tbCIP.Enabled = False
                btnCIP.Enabled = False
                tbJob.Enabled = False
                btnJob.Enabled = False
                btnReff.Enabled = True
                'tbJob.Enabled = True
            Else
                tbKontraktor.Enabled = True
                btnKontraktor.Enabled = True
                tbCIP.Enabled = False
                btnCIP.Enabled = False
                tbJob.Enabled = True
                btnJob.Enabled = True
                tbReffNo.Text = ""
                btnReff.Enabled = False
                tbJob.Text = ""
                tbJobName.Text = ""
                tbUnit.Text = ""
                'btnKontraktor.Enabled = True
                'tbKontraktor.Enabled = True

            End If

            'If ddlReffType.SelectedValue = "Schedule" Then
            '    btnReff.Enabled = True
            '    tbJob.Enabled = False
            'Else
            '    btnReff.Enabled = False
            '    tbReffNo.Text = ""
            '    tbJob.Enabled = True
            'End If
            'btnJob.Enabled = tbJob.Enabled
            'tbRotasi.Enabled = tbJob.Enabled
            'tbCIP.Enabled = btnReff.Enabled
            'tbKontraktor.Enabled = btnReff.Enabled
            'btnCIP.Enabled = btnReff.Enabled
            'btnKontraktor.Enabled = btnReff.Enabled
        Catch ex As Exception
            Throw New Exception("ddlReffType_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.TextChanged
        Try


        Catch ex As Exception
            Throw New Exception("ddlType_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlWorkBy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWorkBy.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try

            Dt = SQLExecuteQuery("EXEC S_FindTeam" + QuotedStr(ddlWorkBy.SelectedValue), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                If Dr("Team_Type") = "Borongan" Then
                    tbKontraktor.Enabled = True
                    btnKontraktor.Enabled = True
                Else
                    tbKontraktor.Enabled = False
                    btnKontraktor.Enabled = False
                End If
                If Dr("Total_Member") > 0 Then
                    tbPerson.Text = Dr("Total_Member")
                Else
                    tbPerson.Text = "1"
                End If
            End If

        Catch ex As Exception
            Throw New Exception("ddlWorkBy_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbPerson_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPerson.TextChanged
        Try
            If pnlEditDt.Visible = True Then
                If tbPercentage.Text > 100 Then tbPercentage.Text = 100
                If tbPercentage.Text < 0 Then tbPercentage.Text = 0
                tbQtyWO.Text = (Val(tbPercentage.Text) / 100) * Val(tbTotal.Text)

                If (tbQtyCapacity.Text > 0) And (tbPerson.Text > 0) Then
                    tbQtyWorkDay.Text = FormatNumber((tbQtyWO.Text) / CFloat(tbQtyCapacity.Text) / CInt(tbPerson.Text), 2)
                    'tbQtyWorkDay.Text = (CFloat(tbQtyTarget.Text) / CInt(tbPerson.Text))
                    tbEndDate.SelectedDate = Date.FromOADate(tbStartDate.SelectedDate.ToOADate() + CInt(tbQtyWorkDay.Text))
                Else : tbQtyWorkDay.Text = 0
                End If

                If (tbQtyCapacity.Text > 0) And (tbQtyWO.Text > 0) Then
                    tbQtyTarget.Text = FormatNumber((tbQtyWO.Text) / CFloat(tbQtyCapacity.Text), 2)
                Else : tbQtyTarget.Text = 0
                End If
            Else
                Dim Row As DataRow
                Dim GVR As GridViewRow
                Dim StartDate As Date
                For Each GVR In GridDt.Rows

                    StartDate = GVR.Cells(12).Text
                    ViewState("Percentage") = 100
                    ViewState("Qty") = GVR.Cells(8).Text
                    ViewState("QtyTotal") = GVR.Cells(7).Text
                    ViewState("Capacity") = GVR.Cells(9).Text
                    ViewState("TargetHK") = GVR.Cells(10).Text
                    ViewState("WorkDay") = GVR.Cells(11).Text

                    If ViewState("Percentage") > 100 Then ViewState("Percentage") = 100
                    If ViewState("Percentage") < 0 Then ViewState("Percentage") = 0
                    'GVR.Cells(8).Text = (Val(GVR.Cells(6).Text) / 100)  * Val(GVR.Cells(7).Text)

                    If (ViewState("Capacity") > 0) And (tbPerson.Text > 0) Then
                        GVR.Cells(11).Text = FormatNumber((GVR.Cells(8).Text) / CFloat(ViewState("Capacity")) / CInt(tbPerson.Text), 2)
                        GVR.Cells(13).Text = Date.FromOADate(StartDate.ToOADate() + CInt(GVR.Cells(11).Text))
                    Else : GVR.Cells(11).Text = 0
                    End If

                    If (ViewState("Capacity") > 0) And (ViewState("Qty") > 0) Then
                        GVR.Cells(10).Text = FormatNumber((ViewState("Qty")) / CFloat(ViewState("Capacity")), 2)
                    Else : GVR.Cells(10).Text = 0
                    End If


                    Row = ViewState("Dt").Select("Type+'|'+DivisiBlok = " + QuotedStr(GVR.Cells(2).Text + "|" + GVR.Cells(3).Text))(0)
                    Row.BeginEdit()
                    Row("NormaHK") = GVR.Cells(9).Text
                    Row("TargetHK") = GVR.Cells(10).Text
                    Row("WorkDay") = GVR.Cells(11).Text
                    Row("EndDate") = GVR.Cells(13).Text
                    Row.EndEdit()

                    'lbStatus.Text = ViewState("WorkDay")
                    'Exit Sub
                    'btnSaveDt_Click(Nothing, Nothing)

                Next

            End If
        Catch ex As Exception
            Throw New Exception("tbPercentage_TextChanged Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub tbPercentage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPercentage.TextChanged
        Try
            If tbPercentage.Text > 100 Then tbPercentage.Text = 100
            If tbPercentage.Text < 0 Then tbPercentage.Text = 0

            tbQtyWO.Text = (Val(tbPercentage.Text) / 100) * Val(tbTotal.Text)

            If (tbQtyCapacity.Text > 0) And (tbPerson.Text > 0) Then
                tbQtyWorkDay.Text = FormatNumber((tbQtyWO.Text) / CFloat(tbQtyCapacity.Text) / CInt(tbPerson.Text), 2)
                'tbQtyWorkDay.Text = (CFloat(tbQtyTarget.Text) / CInt(tbPerson.Text))
                tbEndDate.SelectedDate = Date.FromOADate(tbStartDate.SelectedDate.ToOADate() + CInt(tbQtyWorkDay.Text))
            Else : tbQtyWorkDay.Text = 0
            End If

            If (tbQtyCapacity.Text > 0) And (tbQtyWO.Text > 0) Then
                tbQtyTarget.Text = FormatNumber((tbQtyWO.Text) / CFloat(tbQtyCapacity.Text), 2)
            Else : tbQtyTarget.Text = 0
            End If
        Catch ex As Exception
            Throw New Exception("tbPercentage_TextChanged Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub tbJob_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbJob.TextChanged
        Try
            Dim Dt As DataTable
            Dim Dr As DataRow
            Try

                Dt = SQLExecuteQuery("select Job_Code, Job_Name, Level_Plant, Job_No, Unit, FgCIP From V_MsJobPlant Where Job_Code= " + QuotedStr(tbJob.Text), ViewState("DBConnection").ToString).Tables(0)
                If Dt.Rows.Count > 0 Then
                    Dr = Dt.Rows(0)
                    tbJob.Text = Dr("Job_Code")
                    tbJobName.Text = Dr("Job_Name")
                    btnJob.Focus()
                    If Dr("FgCIP") = "Y" Then
                        tbCIP.Enabled = True
                    Else
                        tbCIP.Enabled = False
                    End If
                Else
                    tbJob.Text = ""
                    tbJobName.Text = ""
                    tbCIP.Enabled = False
                    tbJob.Focus()
                End If
                btnCIP.Enabled = tbCIP.Enabled
            Catch ex As Exception
                Throw New Exception("ddlWorkBy_TextChanged Error : " + ex.ToString)
            End Try
        Catch ex As Exception
            Throw New Exception("tbJob_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbCIP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCIP.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select CIPCode, CIPName, Estate from V_MsCIP WHERE CIPCode =" + QuotedStr(tbCIP.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbCIP.Text = Dr("CIPCode")
                tbCIPName.Text = Dr("CIPName")
                btnCIP.Focus()
            Else
                tbCIP.Text = ""
                tbCIPName.Text = ""
                tbCIP.Focus()
            End If
        Catch ex As Exception
            Throw New Exception("tbCIP_TextChanged Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub tbKontraktor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbKontraktor.TextChanged
        Dim Dt As DataTable
        Dim Dr As DataRow
        Try
            Dt = SQLExecuteQuery("Select Kontraktor_Code, Kontraktor_Name from V_MsKontraktor  WHERE Kontraktor_Code =" + QuotedStr(tbKontraktor.Text), ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                tbKontraktor.Text = Dr("Kontraktor_Code")
                tbKontraktorName.Text = Dr("Kontraktor_Name")
                btnKontraktor.Focus()
            Else
                tbKontraktor.Text = ""
                tbKontraktorName.Text = ""
                tbKontraktor.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tbKontraktor_TextChanged Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbQtyCapacity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbQtyCapacity.TextChanged
        Try
            If (tbQtyCapacity.Text > 0) And (tbPerson.Text > 0) Then
                tbQtyWorkDay.Text = FormatNumber((tbQtyWO.Text) / CFloat(tbQtyCapacity.Text) / CInt(tbPerson.Text), 2)
                tbEndDate.SelectedDate = Date.FromOADate(tbStartDate.SelectedDate.ToOADate() + CInt(tbQtyWorkDay.Text))
            Else : tbQtyWorkDay.Text = 0
            End If

            If (tbQtyCapacity.Text > 0) And (tbQtyWO.Text > 0) Then
                tbQtyTarget.Text = FormatNumber((tbQtyWO.Text) / CFloat(tbQtyCapacity.Text), 2)
            Else : tbQtyTarget.Text = 0
            End If
        Catch ex As Exception
            Throw New Exception("tbQtyCapacity_TextChanged Error : " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnBackDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackDt2.Click, btnBackDt2ke2.Click, BtnBackDt3.Click, BtnBackDt3ke2.Click, BtnBackDt4.Click, btnbackDt4ke2.Click
        Try

            MultiView1.ActiveViewIndex = 0
            pnlDt.Visible = True
            pnlEditDt.Visible = False
            GridDt.Columns(0).Visible = GetCountRecord(ViewState("Dt")) > 0
        Catch ex As Exception
            Throw New Exception("btnBackDt2_Click Error : " + ex.ToString)
        End Try
        '   btnBackDt2ke2
    End Sub

    Private Sub GetMachine(ByVal job As String, ByVal reff As String, ByVal DivisiBlok As String, ByVal Type As String)
        Dim drResult As DataRow
        Dim ExistRow As DataRow()
        Dim dtUnit As DataTable
        Dim drUnit As DataRow
        Dim SQLString As String
        Try
            For Each drResult In Session("Result").Rows
                ExistRow = ViewState("Dt2").Select("DivisiBlok = " + QuotedStr(DivisiBlok) + " AND Type = " + QuotedStr(Type))
                If ExistRow.Count = 0 Then
                    SQLString = "EXEC S_PLWOPlanGetMachineAll " + QuotedStr(reff) + " , " + QuotedStr(job)
                    dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                    If dtUnit.Rows.Count > 0 Then
                        drUnit = dtUnit.Rows(0)
                        Dim dr As DataRow
                        dr = ViewState("Dt2").NewRow
                        dr("Type") = Type
                        dr("DivisiBlok") = DivisiBlok
                        dr("Machine") = drUnit("Machine_Code").ToString
                        dr("MachineName") = drUnit("Machine_Name").ToString
                        dr("EstHour") = FormatFloat(drUnit("Est_Hour").ToString, ViewState("DigitQty"))
                        ViewState("Dt2").Rows.Add(dr)
                    End If

                End If
            Next
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt2")) = 0)
            GridDt2.Columns(1).Visible = GetCountRecord(ViewState("Dt2")) > 0
        Catch ex As Exception
            Throw New Exception("GetMachine Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub GetMaterial(ByVal type As String, ByVal reffNo As String, ByVal job As String, ByVal block As String, ByVal rotasi As Integer, ByVal divBlockType As String, ByVal statusTanam As String, ByVal qty As String, ByVal pkk As String)
        Dim drResult As DataRow
        Dim ExistRow As DataRow()
        Dim dtUnit As DataTable
        Dim drUnit As DataRow
        Dim SQLString As String
        Try

            For Each drResult In Session("Result").Rows
                ExistRow = ViewState("Dt4").Select("DivisiBlok = " + QuotedStr(block) + " AND Type = " + QuotedStr(divBlockType))
                If ExistRow.Count = 0 Then
                    'SQLString = "EXEC S_PLWOPlanFindRMReff " + QuotedStr(type) + " , " + QuotedStr(reffNo) + " , " + QuotedStr(job) + " , " + QuotedStr(block) + " , " + rotasi + " , " + QuotedStr(divBlockType) + " , " + QuotedStr(statusTanam) + " , " + QuotedStr(qty) + " , " + QuotedStr(pkk)
                    SQLString = "EXEC S_PLWOPlanFindRMReff 'Schedule','IAL/PTB/1910/0001','21172','194','1','Batch','2009','6000',0"
                    
                    dtUnit = SQLExecuteQuery(SQLString, ViewState("DBConnection")).Tables(0)
                    

                    If dtUnit.Rows.Count > 0 Then
                        drUnit = dtUnit.Rows(0)
                        Dim dr As DataRow
                        dr = ViewState("Dt4").NewRow
                        dr("Type") = type.ToString
                        dr("DivisiBlok") = block.ToString
                        dr("Material") = drUnit("Material_Code").ToString
                        dr("MaterialName") = drUnit("Material_Name").ToString
                        dr("Qty") = FormatFloat(drUnit("Qty").ToString, ViewState("DigitQty"))
                        dr("Unit") = drUnit("Unit").ToString
                        If qty > 0 Then
                            dr("QtyDosis") = dr("Qty") / qty
                        Else
                            dr("QtyDosis") = 0
                        End If

                        If pkk > 0 Then
                            dr("QtyPokok") = dr("Qty") / pkk
                        Else : dr("QtyPokok") = 0

                            dr("AltQty") = dr("Qty")
                            ViewState("Dt4").Rows.Add(dr)
                        End If
                    End If
                End If
            Next
            BindGridDt(ViewState("Dt4"), GridDt4)
            EnableHd(GetCountRecord(ViewState("Dt4")) = 0)
            GridDt4.Columns(1).Visible = GetCountRecord(ViewState("Dt4")) > 0
        Catch ex As Exception
            Throw New Exception("GetMaterial Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub lbCount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCount.Click
        Dim ResultField, sqlstring, ResultSame, CriteriaField As String
        Try
            'sqlstring = "SELECT * FROM V_PLWOPlanGetReff WHERE REFF_Type = 'Batch' "
            sqlstring = "SELECT A.*, B.Status FROM V_PLWOPlanGetReff A LEFT OUTER JOIN PLWOPlanHd B ON A.TransNmbr = B.Reference And A.JobPlant = B.JobPlant And A.Rotasi = B.MonthNo WHere B.Status Is NULL AND A.Reff_Type = 'Batch' "

            Session("filter") = sqlstring
            ResultField = "Reff_Type, TransNmbr, Rotasi, JobPlant, JobPlant_Name,LevelPlant_Code, Team, Type, DivisiBlok, DivisiBlok_Name, Division, StartDate, EndDate, Qty, Capacity, Unit, Section, RK_Year, RK_Month, PlantPeriodCode, PlantPeriodName, Area, MaxCap"
            CriteriaField = "Reff_Type, TransNmbr,Rotasi, JobPlant, JobPlant_Name,LevelPlant_Code, Team, Type, DivisiBlok, DivisiBlok_Name, Division, StartDate, EndDate, Qty, Capacity, Unit, Section, RK_Year, RK_Month, PlantPeriodCode, PlantPeriodName, Area, MaxCap"
            Session("DBConnection") = ViewState("DBConnection")
            Session("ClickSame") = "TransNmbr,Rotasi, JobPlant,"
            Session("Column") = ResultField.Split(",")
            Session("CriteriaField") = CriteriaField.Split(",")
            ResultSame = "TransNmbr,Rotasi, JobPlant, DivisiBlok"
            Session("ResultSame") = ResultSame.Split(",")
            ViewState("Sender") = "btnOut"
            AttachScript("OpenSearchMultiDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "btn Multi Generate Click Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        'Dim dt As New DataTable
        'Dim drResult, dr As DataRow
        'Dim i As Integer = 1

        'Try
        '    'If Not CekHd() Then
        '    '    Exit Sub
        '    'End If


        '    dt = SQLExecuteQuery("SELECT A.*, B.Status FROM V_PLWOPlanGetReff A LEFT OUTER JOIN PLWOPlanHd B ON A.TransNmbr = B.Reference And A.JobPlant = B.JobPlant And A.Rotasi = B.MonthNo  WHere B.Status Is NULL AND A.Reff_Type = 'Batch' AND A.TransNmbr = " + QuotedStr(tbReffNo.Text) + " And A.Division = " + QuotedStr(ddlDivisi.SelectedValue) + " And A.Rotasi = " + tbRotasi.Text + " And A.JobPlant = " + QuotedStr(tbJob.Text), ViewState("DBConnection").ToString).Tables(0)
        '    For Each drResult In dt.Rows
        '        'ViewState("Dt").Select("DivisiBlok = " + QuotedStr(Divisi) + " AND Type = " + QuotedStr(Type))
        '        If CekExistData(ViewState("Dt"), "DivisiBlok, Type", drResult("DivisiBlok") + "|" + drResult("Type")) = False Then
        '            If ddlReffType.SelectedValue = "Schedule" Then
        '                dr = ViewState("Dt").NewRow
        '                dr("DivisiBlok") = drResult("DivisiBlok").ToString
        '                dr("DivisiBlokName") = drResult("DivisiBlok_Name").ToString
        '                dr("StatusTanam") = drResult("PlantPeriodCode").ToString
        '                dr("StatusTanamName") = drResult("PlantPeriodName").ToString
        '                dr("Type") = drResult("Type").ToString
        '                dr("StartDate") = drResult("StartDate")
        '                dr("EndDate") = drResult("EndDate")
        '                dr("Percentage") = 100
        '                dr("Qty") = FormatFloat(drResult("Qty").ToString, ViewState("DigitQty"))
        '                dr("QtyTotal") = FormatFloat(drResult("Qty").ToString, ViewState("DigitQty"))
        '                dr("NormaHK") = FormatFloat(drResult("Capacity").ToString, ViewState("DigitQty"))

        '                If (dr("NormaHK") > 0) And (tbPerson.Text > 0) Then
        '                    dr("WorkDay") = (drResult("Qty") / dr("NormaHK") / tbPerson.Text)
        '                    '   GetEndDate()
        '                    dr("EndDate") = Date.FromOADate(dr("StartDate").ToOADate() + CInt(dr("WorkDay")))
        '                Else
        '                    dr("WorkDay") = 0
        '                End If

        '                If (dr("NormaHK") > 0) And (dr("Qty") > 0) Then
        '                    dr("TargetHK") = CFloat(dr("Qty").ToString) / CFloat(dr("NormaHK").ToString)
        '                Else : dr("TargetHK") = 0
        '                End If
        '                tbKontraktor.Enabled = False
        '                btnKontraktor.Enabled = False
        '            Else
        '                '"type, DivisiBlokCode, DivisiBlokName, PlantPeriodCode, PlantPeriodName, Qty, Pkk, NormaKH, Area, MaxCap"
        '                dr("Type") = drResult("Type").ToString
        '                dr("DivisiBlok") = drResult("DivisiBlokCode").ToString
        '                dr("DivisiBlokName") = drResult("DivisiBlokName").ToString
        '                dr("StatusTanam") = drResult("PlantPeriodCode").ToString
        '                dr("StatusTanamName") = drResult("PlantPeriodName").ToString
        '                dr("StartDate") = Date.Today
        '                dr("EndDate") = Date.Today
        '                dr("Percentage") = 100
        '                Dim dtUnit As DataTable
        '                Dim drUnit As DataRow
        '                dtUnit = SQLExecuteQuery("EXEC S_PLWOPlanFindDivBlock " + QuotedStr(ddlReffType.SelectedValue) + " , " + QuotedStr(tbReffNo.Text) + "," + QuotedStr(tbJob.Text) + "," + QuotedStr(drResult("Type").ToString) + ", " + QuotedStr(drResult("DivisiBlokCode").ToString) + ", " + QuotedStr(ddlDivisi.SelectedValue) + "," + tbRotasi.Text, ViewState("DBConnection")).Tables(0)
        '                drUnit = dtUnit.Rows(0)

        '                dr("Qty") = FormatFloat(drUnit("Qty").ToString, ViewState("DigitQty"))
        '                dr("Pokok") = FormatFloat(drUnit("Pkk").ToString, ViewState("DigitQty"))
        '                dr("QtyTotal") = FormatFloat(drUnit("Qty").ToString, ViewState("DigitQty"))
        '                dr("NormaHK") = FormatFloat(drUnit("NormaHK").ToString, ViewState("DigitQty"))
        '                If CFloat(drResult("Area").ToString) = 0 Then
        '                    ViewState("SPH") = 0
        '                Else
        '                    ViewState("SPH") = CFloat(drUnit("Pkk").ToString) / CFloat(drResult("Area").ToString)
        '                End If

        '                If (dr("NormaHK") > 0) And (CInt(tbPerson.Text) > 0) Then
        '                    dr("WorkDay") = (CFloat(drUnit("Qty")) / CFloat(dr("NormaHK")) / CInt(tbPerson.Text))
        '                    'GetEndDate()
        '                    dr("EndDate") = Date.FromOADate(dr("StartDate").ToOADate() + CInt(dr("WorkDay")))
        '                Else
        '                    dr("WorkDay") = 0
        '                End If

        '                If (dr("NormaHK") > 0) And (dr("Qty") > 0) Then
        '                    dr("TargetHK") = CFloat(dr("Qty").ToString) / CFloat(dr("NormaHK").ToString)
        '                Else
        '                    dr("TargetHK") = 0
        '                End If

        '            End If
        '            ViewState("Dt").Rows.Add(dr)
        '        End If
        '    Next

        Dim dt2 As New DataTable
        Dim drResult2, dr2 As DataRow
        Dim drow2 As DataRow
        Dim GVR As GridViewRow
        Dim ExistRow2 As DataRow()
        Try

            'For Each drow2 In ViewState("Dt4").rows
            '    drow2.Delete()
            'Next

            For Each GVR In GridDt.Rows
                ExistRow2 = ViewState("Dt4").Select("DivisiBlok = " + QuotedStr(LblBatchMT.Text) + " AND Type = " + QuotedStr(LblTypeMT.Text))
                If ExistRow2.Count = 0 Then
                    'dt2 = SQLExecuteQuery("EXEC S_PLWOPlanFindRMReff 'Schedule','IAL/PTB/1910/0001','21172','194','1','Batch','2009','6000',0", ViewState("DBConnection").ToString).Tables(0)
                    dt2 = SQLExecuteQuery("EXEC S_PLWOPlanFindRMReff " + QuotedStr(ddlReffType.SelectedValue) + "," + QuotedStr(tbReffNo.Text) + _
                    "," + QuotedStr(tbJob.Text) + "," + QuotedStr(GVR.Cells(3).Text) + "," + QuotedStr(tbRotasi.Text) + _
                    "," + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(GVR.Cells(5).Text) + "," + QuotedStr(GVR.Cells(8).Text.Replace(",", "")) + "," + QuotedStr(GVR.Cells(10).Text.Replace(",", "")) + " ", ViewState("DBConnection").ToString).Tables(0)

                    For Each drResult2 In dt2.Rows
                        If CekExistData(ViewState("Dt4"), "Material, Type", drResult2("Material_Code") + "|" + LblTypeMT.Text) = False Then
                            dr2 = ViewState("Dt4").NewRow
                            dr2("Type") = LblTypeMT.Text
                            dr2("DivisiBlok") = LblBatchMT.Text
                            dr2("Material") = drResult2("Material_Code").ToString

                            'lbStatus.Text = dr2("Material")
                            'Exit Sub

                            dr2("MaterialName") = drResult2("Material_Name").ToString
                            dr2("QtyTotal") = tbQty.Text
                            dr2("Qty") = FormatFloat(drResult2("Qty").ToString, ViewState("DigitQty"))
                            dr2("Unit") = drResult2("Unit").ToString
                            If GVR.Cells(8).Text.Replace(",", "") > 0 Then
                                dr2("QtyDosis") = dr2("Qty") / GVR.Cells(8).Text.Replace(",", "")
                            Else
                                dr2("QtyDosis") = 0
                            End If

                            If GVR.Cells(9).Text.Replace(",", "") > 0 Then
                                dr2("QtyPokok") = dr2("Qty") / GVR.Cells(10).Text.Replace(",", "")
                            Else : dr2("QtyPokok") = 0

                                dr2("AltQty") = dr2("Qty")

                            End If
                            ViewState("Dt4").Rows.Add(dr2)
                        End If
                    Next
                    BindGridDt(ViewState("Dt4"), GridDt4)
                    EnableHd(GetCountRecord(ViewState("Dt4")) = 0)
                    GridDt4.Columns(1).Visible = GetCountRecord(ViewState("Dt4")) > 0

                    'BindGridDt(ViewState("Dt"), GridDt)
                    'BindGridDt(ViewState("Dt4"), GridDt4)
                    'EnableHd(GetCountRecord(ViewState("Dt")) = 0)
                    'GridDt.Columns(1).Visible = GetCountRecord(ViewState("Dt")) > 0
                End If
            Next

        Catch ex As Exception
            lbStatus.Text = "Btn Get Data Error : " + ex.ToString
        End Try
    End Sub


End Class