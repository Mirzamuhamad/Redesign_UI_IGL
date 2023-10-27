
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Partial Class Transaction_TrPECandidate_TrPECandidate
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "Select * From V_PECandidateHd"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                SetInit()
                Session("AdvanceFilter") = ""
            End If
            lbStatus.Text = ""
            If Not Session("FgAdvanceFilter") Is Nothing Then
                BindData(Session("AdvanceFilter"))
                Session("FgAdvanceFilter") = Nothing
            End If
            If Not Session("Result") Is Nothing Then

                If ViewState("Sender") = "btnRequestNo" Then
                    'ResultField = "RequestEmp_No, JobTitle_Code, JobTitle_Name, EmpStatus_Code, EmpStatus_Name"
                    tbRequestNo.Text = Session("Result")(0).ToString
                    BindToDropList(ddlJobTitle, Session("Result")(1).ToString)
                    BindToDropList(ddlEmpStatus, Session("Result")(3).ToString)
                End If
                If ViewState("Sender") = "btnEmpReference" Then
                    BindToText(tbEmpReference, Session("Result")(0).ToString)
                    BindToText(tbEmpReferenceName, Session("Result")(1).ToString)
                End If

                'Session("filter") = Nothing
                'Session("Column") = Nothing
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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim index As Integer
        Try
            index = Int32.Parse(e.Item.Value)
            MultiView1.ActiveViewIndex = index
            btnSaveTrans.Focus()
        Catch ex As Exception
            lbStatus.Text = "Menu Item Click Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SetInit()
        Try
            FillRange(ddlRange)
            FillCombo(ddlJobTitle, "EXEC S_GetJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection"))
            FillCombo(ddlEmpStatus, "EXEC S_GetEmpStatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection"))
            FillCombo(ddlReligion, "EXEC S_GetReligion", False, "ReligionCode", "ReligionName", ViewState("DBConnection"))
            FillCombo(ddlResCity, "EXEC S_GetCity", True, "City_Code", "City_Name", ViewState("DBConnection"))
            FillCombo(ddlOriCity, "EXEC S_GetCity", True, "City_Code", "City_Name", ViewState("DBConnection"))
            FillCombo(ddlFamEduLevel, "EXEC S_GetEduLevel", True, "EduLevelCode", "EduLevelName", ViewState("DBConnection"))
            FillCombo(ddlEduLevel, "EXEC S_GetEduLevel", True, "EduLevelCode", "EduLevelName", ViewState("DBConnection"))
            FillCombo(ddlEduCity, "EXEC S_GetCity", True, "City_Code", "City_Name", ViewState("DBConnection"))
            FillCombo(ddlCompanyCity, "EXEC S_GetCity", True, "City_Code", "City_Name", ViewState("DBConnection"))
            FillCombo(ddlCurr, "EXEC S_GetCurrency", True, "Currency", "Currency", ViewState("DBConnection"))

            ViewState("SortExpression") = Nothing
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            FillAction(BtnAdd, btnAdd2, ddlCommand, ddlCommand2, ViewState("MenuLevel").Rows(0))

            tbHeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbWeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbDurasiYear.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGPA.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbGraduateYear.Attributes.Add("OnKeyDown", "return PressNumeric();")
            tbHeight.Attributes.Add("OnBlur", "setformatdt();")
            tbWeight.Attributes.Add("OnBlur", "setformatdt();")

        Catch ex As Exception
            Throw New Exception("Set Init Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim DT As DataTable
        Dim DV As DataView
        Dim StrFilter As String
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
            DT = BindDataTransaction(GetStringHd, StrFilter, ViewState("DBConnection").ToString)
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
        Return "SELECT * From V_PECandidateFamily WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function
    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT * From V_PECandidateEdu WHERE TransNmbr = " + QuotedStr(Nmbr)
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT * From V_PECandidateExp WHERE TransNmbr = " + QuotedStr(Nmbr)
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
                    Result = ExecSPCommandGo(ActionValue, "S_PECandidate", Nmbr(j), CInt(Session(Request.QueryString("KeyId"))("Year")), CInt(Session(Request.QueryString("KeyId"))("Period")), ViewState("UserId").ToString, ViewState("DBConnection").ToString)
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
            tbDate.Enabled = State
            tbSource.Enabled = State
            tbEmpName.Enabled = State
            ddlGender.Enabled = State
            tbBirthPlace.Enabled = State
            tbBirthDate.Enabled = State
            ddlReligion.Enabled = State
            ddlMaritalSt.Enabled = State
            tbHeight.Enabled = State
            tbWeight.Enabled = State
            ddlTypeCard.Enabled = State
            tbIDCard.Enabled = State
            tbHandPhone.Enabled = State
            tbEmail.Enabled = State
            tbPenyakit.Enabled = State
            tbEmpReference.Enabled = State
            tbResAddr1.Enabled = State
            tbResAddr2.Enabled = State
            ddlResCity.Enabled = State
            tbResZipCode.Enabled = State
            tbResPhone.Enabled = State
            tbOriAddr1.Enabled = State
            tbOriAddr2.Enabled = State
            tbOriZipCode.Enabled = State
            tbOriPhone.Enabled = State
            
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
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
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            BindGridDt(dt, GridDt2)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt3(ByVal Nmbr As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Nmbr), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            BindGridDt(dt, GridDt3)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
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
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt2.Click
        Try
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
            StatusButtonSave(True)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt2  Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            tbSource.Text = ""
            tbRequestNo.Text = ""
            ddlJobTitle.SelectedValue = ""
            ddlEmpStatus.SelectedValue = ""
            tbEmpName.Text = ""
            ddlGender.SelectedIndex = 0
            tbBirthPlace.Text = ""
            tbBirthDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlReligion.SelectedIndex = 0
            ddlMaritalSt.SelectedIndex = 0
            tbHeight.Text = "0"
            tbWeight.Text = "0"
            ddlTypeCard.SelectedIndex = 0
            tbIDCard.Text = ""
            tbHandPhone.Text = ""
            tbEmail.Text = ""
            tbPenyakit.Text = ""
            tbEmpReference.Text = ""
            tbEmpReferenceName.Text = ""
            tbResAddr1.Text = ""
            tbResAddr2.Text = ""
            ddlResCity.SelectedValue = ""
            tbResZipCode.Text = ""
            tbResPhone.Text = ""
            tbOriAddr1.Text = ""
            tbOriAddr2.Text = ""
            ddlOriCity.SelectedValue = ""
            tbOriZipCode.Text = ""
            tbOriPhone.Text = ""

        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt()
        Try
            ddlFamilyType.SelectedIndex = 0
            tbFamilyName.Text = ""
            tbFamBirthPlace.Text = ""
            tbFamBirthDate.SelectedDate = ViewState("ServerDate") 'Now.Date
            ddlFamGender.SelectedIndex = 0
            ddlFamEduLevel.SelectedIndex = 0
            tbFamAddr1.Text = ""
            tbFamAddr2.Text = ""
            tbFamPhone.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub Cleardt2()
        Try
            ddlEduLevel.SelectedIndex = 0
            tbSchoolName.Text = ""
            ddlEduCity.SelectedValue = ""
            tbEduMajor.Text = ""
            tbCertificateNo.Text = ""
            tbDurasiYear.Text = "0"
            tbGPA.Text = "0"
            ddlFgGraduate.SelectedValue = "Y"
            tbGraduateYear.Text = Year(ViewState("ServerDate"))
        Catch ex As Exception
            Throw New Exception("Clear Dt 2 Error " + ex.ToString)
        End Try
    End Sub
    Private Sub Cleardt3()
        Try
            tbCompanyName.Text = ""
            tbCompanyBusiness.Text = ""
            tbCompanyAddr1.Text = ""
            tbCompanyAddr2.Text = ""
            ddlCompanyCity.SelectedValue = ""
            tbCompanyPhone.Text = ""
            tbJobTitle.Text = ""
            tbJobResponsibilty.Text = ""
            tbDepartment.Text = ""
            tbWorkPeriod.Text = ""
            ddlCurr.SelectedValue = ViewState("Currency")
            tbLastSalary.Text = "0"
            tbPHKReason.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt 3 Error " + ex.ToString)
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Date must have value")
                tbDate.Focus()
                Return False
            End If
            'If CInt(Session(Request.QueryString("KeyId"))("Year")) <> Year(tbDate.SelectedValue) Or CInt(Session(Request.QueryString("KeyId"))("Period")) <> Month(tbDate.SelectedValue) Then
            '    lbStatus.Text = MessageDlg("Date must be inputed in accounting period " + MonthName(Session(Request.QueryString("KeyId"))("Period")) + " " + Session(Request.QueryString("KeyId"))("Year").ToString.Trim)
            '    Return False
            'End If
            If tbRequestNo.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Request No must have value")
                tbRequestNo.Focus()
                Return False
            End If
            If tbEmpName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Emp. Name must have value")
                tbEmpName.Focus()
                Return False
            End If
            If tbBirthDate.SelectedDate = Nothing Then
                lbStatus.Text = MessageDlg("Birth Date must have value")
                tbBirthDate.Focus()
                Return False
            End If
            If ddlReligion.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Religion must have value")
                ddlReligion.Focus()
                Return False
            End If
            If tbIDCard.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("ID Card must have value")
                tbIDCard.Focus()
                Return False
            End If
            If tbHandPhone.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("HandPhone must have value")
                tbHandPhone.Focus()
                Return False
            End If
            If tbResAddr1.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Address Resident must have value")
                tbResAddr1.Focus()
                Return False
            End If
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
                If Dr("FamilyType").ToString = "" Then
                    lbStatus.Text = MessageDlg("Family Type Must Have Value")
                    Return False
                End If

            Else
                If ddlFamilyType.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Family Type Must Have Value")
                    ddlFamilyType.Focus()
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
                If Dr("EduLevel").ToString = "" Then
                    lbStatus.Text = MessageDlg("Education Level Must Have Value")
                    Return False
                End If
                If Dr("SchoolName").ToString = "" Then
                    lbStatus.Text = MessageDlg("School Name Must Have Value")
                    Return False
                End If
                If (Dr("FgGraduated").ToString = "Y") And (Dr("Graduated").ToString = "") Then
                    lbStatus.Text = MessageDlg("Graduated Year Must Have Value")
                    Return False
                End If

            Else

                If ddlEduLevel.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Education Level Must Have Value")
                    ddlEduLevel.Focus()
                    Return False
                End If

                If tbSchoolName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("School Name Must Have Value")
                    tbSchoolName.Focus()
                    Return False
                End If

                If (ddlFgGraduate.Text.Trim = "Y") And (tbGraduateYear.Text.Trim = "") Then
                    lbStatus.Text = MessageDlg("Graduated Year Must Have Value")
                    tbGraduateYear.Focus()
                    Return False
                End If

            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt2 Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToDate(tbDate, Dt.Rows(0)("TransDate").ToString)
            BindToText(tbRequestNo, Dt.Rows(0)("RequestNo").ToString)
            BindToDropList(ddlJobTitle, Dt.Rows(0)("JobTitle").ToString)
            BindToDropList(ddlEmpStatus, Dt.Rows(0)("EmpStatus").ToString)
            BindToText(tbSource, Dt.Rows(0)("Source").ToString)
            BindToText(tbEmpName, Dt.Rows(0)("EmpName").ToString)
            BindToDropList(ddlGender, Dt.Rows(0)("Gender").ToString)
            BindToText(tbBirthPlace, Dt.Rows(0)("BirthPlace").ToString)
            BindToDropList(ddlTypeCard, Dt.Rows(0)("TypeCard").ToString)
            BindToDate(tbBirthDate, Dt.Rows(0)("BirthDate").ToString)
            BindToText(tbIDCard, Dt.Rows(0)("IDCard").ToString)
            BindToDropList(ddlReligion, Dt.Rows(0)("Religion").ToString)
            BindToText(tbWeight, Dt.Rows(0)("Weight").ToString)
            BindToText(tbHeight, Dt.Rows(0)("Height").ToString)
            BindToDropList(ddlMaritalSt, Dt.Rows(0)("MaritalSt").ToString)
            BindToText(tbHandPhone, Dt.Rows(0)("HandPhone").ToString)
            BindToText(tbEmail, Dt.Rows(0)("Email").ToString)
            BindToText(tbResAddr1, Dt.Rows(0)("ResAddr1").ToString)
            BindToText(tbResAddr2, Dt.Rows(0)("ResAddr2").ToString)
            BindToText(tbResZipCode, Dt.Rows(0)("ResZipCode").ToString)
            BindToDropList(ddlResCity, Dt.Rows(0)("ResCity").ToString)
            BindToText(tbResPhone, Dt.Rows(0)("ResPhone").ToString)
            BindToText(tbOriAddr1, Dt.Rows(0)("OriAddr1").ToString)
            BindToText(tbOriAddr2, Dt.Rows(0)("OriAddr2").ToString)
            BindToText(tbOriZipCode, Dt.Rows(0)("OriZipCode").ToString)
            BindToDropList(ddlOriCity, Dt.Rows(0)("OriCity").ToString)
            BindToText(tbOriPhone, Dt.Rows(0)("OriPhone").ToString)
            BindToText(tbPenyakit, Dt.Rows(0)("Penyakit").ToString)
            BindToText(tbEmpReference, Dt.Rows(0)("EmpReference").ToString)
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub FillTextBoxDt(ByVal RRNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt").select("ItemNo = " + RRNo)
            If Dr.Length > 0 Then
                lbItemNo.Text = RRNo.ToString
                BindToDropList(ddlFamilyType, Dr(0)("FamilyType").ToString)
                BindToText(tbFamilyName, Dr(0)("FamilyName").ToString)
                BindToText(tbFamBirthPlace, Dr(0)("BirthPlace").ToString)
                BindToDate(tbFamBirthDate, Dr(0)("BirthDate").ToString)
                BindToDropList(ddlFamGender, Dr(0)("Gender").ToString)
                BindToDropList(ddlFamEduLevel, Dr(0)("EduLevel").ToString)
                BindToText(tbFamOccupation, Dr(0)("Occupation").ToString)
                BindToText(tbFamAddr1, Dr(0)("Address1").ToString)
                BindToText(tbFamAddr2, Dr(0)("Address2").ToString)
                BindToText(tbFamPhone, Dr(0)("Phone").ToString)                
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt2(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt2").select("EduLevel = " + QuotedStr(ItemNo))
            If Dr.Length > 0 Then
                BindToDropList(ddlEduLevel, Dr(0)("EduLevel").ToString)
                BindToText(tbSchoolName, Dr(0)("SchoolName").ToString)
                BindToDropList(ddlEduCity, Dr(0)("EduCity").ToString)
                BindToText(tbEduMajor, Dr(0)("EduMajor").ToString)
                BindToText(tbCertificateNo, Dr(0)("CertificateNo").ToString)
                BindToText(tbDurasiYear, Dr(0)("DurasiYear").ToString)
                BindToText(tbGPA, Dr(0)("GPA").ToString)
                BindToDropList(ddlFgGraduate, Dr(0)("FgGraduated").ToString)
                BindToText(tbGraduateYear, Dr(0)("Graduated").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 2 error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub FillTextBoxDt3(ByVal ItemNo As String)
        Dim Dr As DataRow()
        Try
            Dr = ViewState("Dt3").select("ItemNo = " + ItemNo)
            If Dr.Length > 0 Then
                lbItemNoDt3.Text = ItemNo.ToString
                BindToText(tbCompanyName, Dr(0)("CompanyName").ToString)
                BindToText(tbCompanyBusiness, Dr(0)("CompanyBusiness").ToString)
                BindToText(tbCompanyAddr1, Dr(0)("Address1").ToString)
                BindToText(tbCompanyAddr2, Dr(0)("Address2").ToString)
                BindToDropList(ddlCompanyCity, Dr(0)("CompanyCity").ToString)
                BindToText(tbCompanyPhone, Dr(0)("CompanyPhone").ToString)
                BindToText(tbJobTitle, Dr(0)("JobTitle").ToString)
                BindToText(tbDepartment, Dr(0)("Department").ToString)
                BindToText(tbJobResponsibilty, Dr(0)("JobResponsibilty").ToString)
                BindToText(tbWorkPeriod, Dr(0)("WorkPeriod").ToString)
                BindToDropList(ddlCurr, Dr(0)("CurrCode").ToString)
                BindToText(tbLastSalary, Dr(0)("LastSalary").ToString)
                BindToText(tbPHKReason, Dr(0)("PHKReason").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("fill text box detail 3 error : " + ex.ToString)
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
                Row = ViewState("Dt").Select("ItemNo = " + lbItemNo.Text)(0)
                Row.BeginEdit()
                Row("ItemNo") = lbItemNo.Text
                Row("FamilyType") = ddlFamilyType.SelectedValue
                Row("FamilyName") = tbFamilyName.Text
                Row("BirthPlace") = tbFamBirthPlace.Text
                Row("BirthDate") = tbFamBirthDate.SelectedDate
                Row("Gender") = ddlFamGender.SelectedValue
                Row("EduLevel") = ddlFamEduLevel.SelectedValue
                Row("Occupation") = tbFamOccupation.Text
                Row("Address1") = tbFamAddr1.Text
                Row("Address2") = tbFamAddr2.Text
                Row("Phone") = tbFamPhone.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt").NewRow
                dr("ItemNo") = lbItemNo.Text
                dr("FamilyType") = ddlFamilyType.SelectedValue
                dr("FamilyName") = tbFamilyName.Text
                dr("BirthPlace") = tbFamBirthPlace.Text
                dr("BirthDate") = tbFamBirthDate.SelectedDate
                dr("Gender") = ddlFamGender.SelectedValue
                dr("EduLevel") = ddlFamEduLevel.SelectedValue
                dr("Occupation") = tbFamOccupation.Text
                dr("Address1") = tbFamAddr1.Text
                dr("Address2") = tbFamAddr2.Text
                dr("Phone") = tbFamPhone.Text
                ViewState("Dt").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt, PnlDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0)
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
                Row = ViewState("Dt2").Select("EduLevel = " + QuotedStr(ddlEduLevel.SelectedValue))(0)
                Row.BeginEdit()
                Row("EduLevel") = ddlEduLevel.SelectedValue
                Row("SchoolName") = tbSchoolName.Text
                Row("EduCity") = ddlEduCity.SelectedValue
                Row("EduMajor") = tbEduMajor.Text
                Row("CertificateNo") = tbCertificateNo.Text
                Row("DurasiYear") = tbDurasiYear.Text
                Row("GPA") = tbGPA.Text
                Row("FgGraduated") = ddlFgGraduate.SelectedValue
                Row("Graduated") = tbGraduateYear.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow

                dr = ViewState("Dt2").NewRow
                dr("EduLevel") = ddlEduLevel.SelectedValue
                dr("SchoolName") = tbSchoolName.Text
                dr("EduCity") = ddlEduCity.SelectedValue
                dr("EduMajor") = tbEduMajor.Text
                dr("CertificateNo") = tbCertificateNo.Text
                dr("DurasiYear") = tbDurasiYear.Text
                dr("GPA") = tbGPA.Text
                dr("FgGraduated") = ddlFgGraduate.SelectedValue
                dr("Graduated") = tbGraduateYear.Text
                ViewState("Dt2").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt2, pnlDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
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
                tbCode.Text = GetAutoNmbr("CD", "Y", Year(tbDate.SelectedValue), Month(tbDate.SelectedValue), "", ViewState("DBConnection").ToString)

                SQLString = "INSERT INTO PECandidateHd (TransNmbr, TransDate, STATUS, " + _
                "RequestNo, JobTitle, EmpStatus, Source, EmpName, Gender, " + _
                "BirthPlace, BirthDate, TypeCard, IDCard, Religion, " + _
                "Weight, Height, MaritalSt, HandPhone, Email, " + _
                "ResAddr1, ResAddr2, ResZipCode, ResCity, ResPhone, " + _
                "OriAddr1, OriAddr2, OriZipCode, OriCity, OriPhone, Penyakit, EmpReference, " + _
                "UserPrep, DatePrep) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + ", 'H', " + _
                QuotedStr(tbRequestNo.Text) + "," + QuotedStr(ddlJobTitle.SelectedValue) + "," + QuotedStr(ddlEmpStatus.SelectedValue) + "," + _
                QuotedStr(tbSource.Text) + "," + QuotedStr(tbEmpName.Text) + "," + QuotedStr(ddlGender.SelectedValue) + "," + _
                QuotedStr(tbBirthPlace.Text) + "," + QuotedStr(Format(tbBirthDate.SelectedValue, "yyyy-MM-dd")) + "," + QuotedStr(ddlTypeCard.SelectedValue) + "," + _
                QuotedStr(tbIDCard.Text) + "," + QuotedStr(ddlReligion.SelectedValue) + "," + _
                tbWeight.Text + "," + tbHeight.Text + "," + QuotedStr(ddlMaritalSt.SelectedValue) + "," + _
                QuotedStr(tbHandPhone.Text) + "," + QuotedStr(tbEmail.Text) + "," + _
                QuotedStr(tbResAddr1.Text) + "," + QuotedStr(tbResAddr2.Text) + "," + QuotedStr(tbResZipCode.Text) + "," + _
                QuotedStr(ddlResCity.SelectedValue) + "," + QuotedStr(tbResPhone.Text) + "," + _
                QuotedStr(tbOriAddr1.Text) + "," + QuotedStr(tbOriAddr2.Text) + "," + QuotedStr(tbOriZipCode.Text) + "," + _
                QuotedStr(ddlOriCity.SelectedValue) + "," + QuotedStr(tbOriPhone.Text) + "," + _
                QuotedStr(tbPenyakit.Text) + "," + QuotedStr(tbEmpReference.Text) + "," + _
                QuotedStr(ViewState("UserId").ToString) + ", GetDate()"
                ViewState("TransNmbr") = tbCode.Text

            Else
                Dim cekStatus As String
                cekStatus = SQLExecuteScalar("Select Status FROM PECandidateHd WHERE TransNmbr = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
                If cekStatus = "P" Then
                    lbStatus.Text = MessageDlg("Save failed. Data has already posted by another user")
                    Exit Sub
                End If
                SQLString = "UPDATE PECandidateHd SET TransDate = " + QuotedStr(Format(tbDate.SelectedValue, "yyyy-MM-dd")) + _
                ", RequestNo = " + QuotedStr(tbRequestNo.Text) + ", JobTitle = " + QuotedStr(ddlJobTitle.SelectedValue) + _
                ", EmpStatus = " + QuotedStr(ddlEmpStatus.SelectedValue) + ", Source = " + QuotedStr(tbSource.Text) + _
                ", EmpName = " + QuotedStr(tbEmpName.Text) + ", Gender = " + QuotedStr(ddlGender.SelectedValue) + _
                ", BirthPlace = " + QuotedStr(tbBirthPlace.Text) + ", BirthDate = " + QuotedStr(Format(tbBirthDate.SelectedValue, "yyyy-MM-dd")) + _
                ", TypeCard = " + QuotedStr(ddlTypeCard.SelectedValue) + ", IDCard = " + QuotedStr(tbIDCard.Text) + _
                ", Religion = " + QuotedStr(ddlReligion.SelectedValue) + _
                ", Weight = " + tbWeight.Text + ", Height = " + tbHeight.Text + _
                ", MaritalSt = " + QuotedStr(ddlMaritalSt.SelectedValue) + _
                ", HandPhone = " + QuotedStr(tbHandPhone.Text) + ", Email = " + QuotedStr(tbEmail.Text) + _
                ", ResAddr1 = " + QuotedStr(tbResAddr1.Text) + ", ResAddr2 = " + QuotedStr(tbResAddr2.Text) + _
                ", ResZipCode = " + QuotedStr(tbResZipCode.Text) + ", ResCity = " + QuotedStr(ddlResCity.SelectedValue) + _
                ", ResPhone = " + QuotedStr(tbResPhone.Text) + _
                ", OriAddr1 = " + QuotedStr(tbOriAddr1.Text) + ", OriAddr2 = " + QuotedStr(tbOriAddr2.Text) + _
                ", OriZipCode = " + QuotedStr(tbOriZipCode.Text) + ", OriCity = " + QuotedStr(ddlOriCity.SelectedValue) + _
                ", OriPhone = " + QuotedStr(tbOriPhone.Text) + _
                ", Penyakit = " + QuotedStr(tbPenyakit.Text) + ", EmpReference = " + QuotedStr(tbEmpReference.Text) + _
                ", DatePrep = GetDate()" + _
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

            'save dt
            Dim ConnString As String = ViewState("DBConnection").ToString
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT TransNmbr, ItemNo, FamilyType, FamilyName, BirthPlace, BirthDate, Gender, EduLevel, Occupation, Address1, Address2, Phone FROM PECandidateFamily WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("PECandidateFamily")

            Dt = ViewState("Dt")
            da.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt

            'save dt2
            cmdSql = New SqlCommand("SELECT TransNmbr, EduLevel, SchoolName, EduCity, EduMajor, CertificateNo, DurasiYear, GPA, FgGraduated, Graduated FROM PECandidateEdu WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt2 As New DataTable("PECandidateEdu")

            Dt2 = ViewState("Dt2")
            da.Update(Dt2)
            Dt2.AcceptChanges()
            ViewState("Dt2") = Dt2

            'save dt3
            cmdSql = New SqlCommand("SELECT TransNmbr, ItemNo, CompanyName, CompanyBusiness, WorkPeriod, Address1, Address2, CompanyCity, CompanyPhone, JobTitle, Department, JobResponsibilty, CurrCode, LastSalary, PHKReason FROM PECandidateExp WHERE TransNmbr = " + QuotedStr(ViewState("TransNmbr")), con)
            da = New SqlDataAdapter(cmdSql)
            dbcommandBuilder = New SqlCommandBuilder(da)
            da.InsertCommand = dbcommandBuilder.GetInsertCommand
            da.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt3 As New DataTable("PECandidateExp")

            Dt3 = ViewState("Dt3")
            da.Update(Dt3)
            Dt3.AcceptChanges()
            ViewState("Dt3") = Dt3
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
                lbStatus.Text = MessageDlg("Detail Family must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Education must have at least 1 record")
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt3")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Experience must have at least 1 record")
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
            MovePanel(PnlHd, pnlInput)
            ModifyInput2(True, pnlInput, PnlDt, GridDt)
            ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
            ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
            newTrans()
            btnHome.Visible = False
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdddt.Click, btnAddDtke2.Click
        Try
            Cleardt()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt") = "Insert"
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            StatusButtonSave(False)
            lbItemNo.Text = GetNewItemNo(ViewState("Dt"))
            ddlFamilyType.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt2.Click, btnAddDt2Ke2.Click
        Try
            Cleardt2()
            If CekHd() = False Then
                Exit Sub
            End If
            ViewState("StateDt2") = "Insert"
            MovePanel(pnlDt2, pnlEditDt2)
            EnableHd(False)
            StatusButtonSave(False)
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
            PnlDt.Visible = True
            BindDataDt("")
            BindDataDt2("")
            BindDataDt3("")
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
            FDateName = "Candidate Date"
            FDateValue = "TransDate"
            FilterName = "Candidate No, Candidate Date, RequestNo, JobTitle, Emp. Status, Source, Emp. Name, Gender, Birth Place, Birth Date, Type Card, ID. Card, Religion, Weight, Height, Marital Status, HandPhone, Email, Res. Addr1, Res. Addr2, Res. ZipCode, Res. City, Res. Phone, Ori. Addr1, Ori. Addr2, Ori. ZipCode, Ori. City, Ori. TelePhone, Sickness, Emp. Reference"
            FilterValue = "TransNmbr, dbo.FormatDate(TransDate) , RequestNo, JobTitle, EmpStatus, Source, EmpName, Gender, BirthPlace, BirthDate, TypeCard, IDCard, Religion, Weight, Height, MaritalSt, HandPhone, Email, ResAddr1, ResAddr2, ResZipCode, ResCity, ResPhone, OriAddr1, OriAddr2, OriZipCode, OriCity, OriPhone, Penyakit, EmpReference"
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
                    ViewState("StateHd") = "View"
                    ModifyInput2(False, pnlInput, PnlDt, GridDt)
                    ModifyInput2(False, pnlInput, pnlDt2, GridDt2)
                    ModifyInput2(False, pnlInput, pnlDt3, GridDt3)
                    MultiView1.ActiveViewIndex = 0
                    Menu1.Items.Item(0).Selected = True
                    btnHome.Visible = True
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(3).Text = "H" Or GVR.Cells(3).Text = "G" Then
                        CekMenu = CheckMenuLevel("Edit", ViewState("MenuLevel").Rows(0))
                        If CekMenu <> "" Then
                            lbStatus.Text = CekMenu
                            Exit Sub
                        End If
                        MovePanel(PnlHd, pnlInput)
                        ViewState("TransNmbr") = GVR.Cells(2).Text
                        GridDt.PageIndex = 0
                        BindDataDt(ViewState("TransNmbr"))
                        BindDataDt2(ViewState("TransNmbr"))
                        BindDataDt3(ViewState("TransNmbr"))
                        FillTextBoxHd(ViewState("TransNmbr"))
                        ViewState("StateHd") = "Edit"
                        ModifyInput2(True, pnlInput, PnlDt, GridDt)
                        ModifyInput2(True, pnlInput, pnlDt2, GridDt2)
                        ModifyInput2(True, pnlInput, pnlDt3, GridDt3)
                        btnHome.Visible = False
                        MultiView1.ActiveViewIndex = 0
                        Menu1.Items.Item(0).Selected = True
                        EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
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
                        Session("SelectCommand") = "EXEC S_PEFormCandidate " + QuotedStr(GVR.Cells(2).Text) + "," + QuotedStr(ViewState("UserId"))
                        Session("ReportFile") = ".../../../Rpt/FormPECandidate.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg();", Page, Me.GetType)
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

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        Try
            Dim dr As DataRow()
            Dim r As DataRow
            Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
            dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
            For Each r In dr
                r.Delete()
            Next
            BindGridDt(ViewState("Dt"), GridDt)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt2_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt2.RowDeleting
        Try
            Dim dr() As DataRow
            Dim GVR As GridViewRow
            GVR = GridDt2.Rows(e.RowIndex)
            dr = ViewState("Dt2").Select("EduLevel = " + QuotedStr(GVR.Cells(1).Text))
            dr(0).Delete()
            BindGridDt(ViewState("Dt2"), GridDt2)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 2 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt.Rows(e.NewEditIndex)
            'row = ViewState("Dt").Rows(e.NewEditIndex)
            FillTextBoxDt(GVR.Cells(1).Text)
            MovePanel(PnlDt, pnlEditDt)
            EnableHd(False)
            ViewState("StateDt") = "Edit"
            btnSaveDt.Focus()
            StatusButtonSave(False)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridDt2_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt2.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt2.Rows(e.NewEditIndex)
            FillTextBoxDt2(GVR.Cells(1).Text)
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
                lbStatus.Text = MessageDlg("Detail Family must have at least 1 record")
                Exit Sub
            End If
            If GetCountRecord(ViewState("Dt2")) = 0 Then
                lbStatus.Text = MessageDlg("Detail Education must have at least 1 record")
                Exit Sub
            End If
            'If GetCountRecord(ViewState("Dt3")) = 0 Then
            '    lbStatus.Text = MessageDlg("Detail Experience must have at least 1 record")
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
            BtnAdd_Click(Nothing, Nothing)
        Catch ex As Exception
            lbStatus.Text = "btnSaveTrans Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnRequestNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequestNo.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_PECandidateGetRequestEmp"
            ResultField = "RequestEmp_No, JobTitle_Code, JobTitle_Name, EmpStatus_Code, EmpStatus_Name"
            ViewState("Sender") = "btnRequestNo"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn RequestNo Click Error : " + ex.ToString
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
            lbItemNoDt3.Text = GetNewItemNo(ViewState("Dt3"))
            tbCompanyName.Focus()
        Catch ex As Exception
            lbStatus.Text = "btn add dt3 error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
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
                Row = ViewState("Dt3").Select("ItemNo = " + lbItemNoDt3.Text)(0)
                Row.BeginEdit()
                Row("ItemNo") = lbItemNoDt3.Text
                Row("CompanyName") = tbCompanyName.Text
                Row("CompanyBusiness") = tbCompanyBusiness.Text
                Row("Address1") = tbCompanyAddr1.Text
                Row("Address2") = tbCompanyAddr2.Text
                If ddlCompanyCity.SelectedValue = "" Then
                    Row("CompanyCity") = DBNull.Value
                Else
                    Row("CompanyCity") = ddlCompanyCity.SelectedValue
                End If

                Row("CompanyPhone") = tbCompanyPhone.Text
                Row("JobTitle") = tbJobTitle.Text
                Row("Department") = tbDepartment.Text
                Row("JobResponsibilty") = tbJobResponsibilty.Text
                Row("WorkPeriod") = tbWorkPeriod.Text
                Row("CurrCode") = ddlCurr.SelectedValue
                Row("LastSalary") = tbLastSalary.Text
                Row("PHKReason") = tbPHKReason.Text
                Row.EndEdit()
            Else
                'Insert
                Dim dr As DataRow
                dr = ViewState("Dt3").NewRow
                dr("ItemNo") = lbItemNoDt3.Text
                dr("CompanyName") = tbCompanyName.Text
                dr("CompanyBusiness") = tbCompanyBusiness.Text
                dr("Address1") = tbCompanyAddr1.Text
                dr("Address2") = tbCompanyAddr2.Text
                dr("CompanyCity") = ddlCompanyCity.SelectedValue.ToString
                dr("CompanyPhone") = tbCompanyPhone.Text
                dr("JobTitle") = tbJobTitle.Text
                dr("Department") = tbDepartment.Text
                dr("JobResponsibilty") = tbJobResponsibilty.Text
                dr("WorkPeriod") = tbWorkPeriod.Text
                dr("CurrCode") = ddlCurr.SelectedValue
                dr("LastSalary") = tbLastSalary.Text
                dr("PHKReason") = tbPHKReason.Text
                ViewState("Dt3").Rows.Add(dr)
            End If
            MovePanel(pnlEditDt3, pnlDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
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
                If Dr("CompanyName").ToString = "" Then
                    lbStatus.Text = MessageDlg("Company Name Must Have Value")
                    Return False
                End If

            Else
                If tbCompanyName.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Company Name Must Have Value")
                    tbCompanyName.Focus()
                    Return False
                End If

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
            dr = ViewState("Dt3").Select("ItemNo = " + GVR.Cells(1).Text)
            dr(0).Delete()
            BindGridDt(ViewState("Dt3"), GridDt3)
            EnableHd(GetCountRecord(ViewState("Dt")) = 0 And GetCountRecord(ViewState("Dt2")) = 0 And GetCountRecord(ViewState("Dt3")) = 0)
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Row Deleting Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            GVR = GridDt3.Rows(e.NewEditIndex)
            FillTextBoxDt3(GVR.Cells(1).Text)
            MovePanel(pnlDt3, pnlEditDt3)
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            StatusButtonSave(False)
            btnSaveDt3.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid dt3 Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnEmpReference_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmpReference.Click
        Dim ResultField As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * FROM V_MsEmployee WHERE COALESCE(Fg_Active,'N') = 'Y'  "
            ResultField = "Emp_No, Emp_Name"
            ViewState("Sender") = "btnEmpReference"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType())
        Catch ex As Exception
            lbStatus.Text = "Btn EmpReference Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEmpReference_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEmpReference.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Employee", tbEmpReference.Text, ViewState("DBConnection"))
            If Not Dr Is Nothing Then
                tbEmpReference.Text = Dr("Emp_No")
                tbEmpReferenceName.Text = Dr("Emp_Name")
            Else
                tbEmpReference.Text = ""
                tbEmpReferenceName.Text = ""
            End If
        Catch ex As Exception
            lbStatus.Text = "tb EmpReference Text Changed Error : " + ex.ToString
        End Try
    End Sub
End Class
