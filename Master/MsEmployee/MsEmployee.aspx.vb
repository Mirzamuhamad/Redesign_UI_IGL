Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Master_MsEmployee_MsEmployee
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da, da1 As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * From V_MsEmployeeMain "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SortExpression") = Nothing
                FillCombo(ddlEmpStatus, "SELECT EmpStatusCode, EmpStatusName From MsEmpstatus", True, "EmpStatusCode", "EmpStatusName", ViewState("DBConnection").ToString)
                FillCombo(ddlWorkPlace, "SELECT WorkPlaceCode, WorkPlaceName From MsWorkPlace", True, "WorkPlaceCode", "WorkPlaceName", ViewState("DBConnection").ToString)
                FillCombo(ddlReligion, "Select ReligionCode, ReligionName From MsReligion", True, "ReligionCode", "ReligionName", ViewState("DBConnection").ToString)
                FillCombo(ddlResCity, "SELECT CityCode, CityName From MsCity", True, "CityCode", "CityName", ViewState("DBConnection").ToString)
                FillCombo(ddlOriCity, "SELECT CityCode, CityName From MsCity", True, "CityCode", "CityName", ViewState("DBConnection").ToString)
                FillCombo(ddlCityEdu, "SELECT CityCode, CityName From MsCity", True, "CityCode", "CityName", ViewState("DBConnection").ToString)
                FillCombo(ddlCityExp, "SELECT CityCode, CityName From MsCity", True, "CityCode", "CityName", ViewState("DBConnection").ToString)
                FillCombo(ddlJobTitle, "SELECT JobTtlCode, JobTtlName From MsJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection").ToString)
                FillCombo(ddlJobTitleExp, "SELECT JobTtlCode, JobTtlName From MsJobTitle", True, "JobTtlCode", "JobTtlName", ViewState("DBConnection").ToString)
                FillCombo(ddlJobLevel, "SELECT JoblvlCode, JoblvlName From MsJoblevel", True, "JoblvlCode", "JoblvlName", ViewState("DBConnection").ToString)
                FillCombo(ddlMethod, "SELECT MethodCode, MethodName From MsMethod", True, "MethodCode", "MethodName", ViewState("DBConnection").ToString)
                'FillCombo(ddlsection, "SELECT Section_Code, Section_Name From VMsSection", True, "Section_Code", "Section_Name", ViewState("DBConnection").ToString)
                'FillCombo(ddlSubSection, "SELECT Sub_Section_Code, Sub_Section_Name From VMsSubSection", True, "Sub_Section_Code", "Sub_Section_Name", ViewState("DBConnection").ToString)
                FillCombo(ddlDeptExp, "SELECT Deptcode, deptname From Msdepartment", True, "DeptCode", "deptName", ViewState("DBConnection").ToString)
                FillCombo(ddlEduLevel, "SELECT EduLevelCode, EduLevelName From MsEduLevel", True, "EduLevelCode", "EduLevelName", ViewState("DBConnection").ToString)
                FillCombo(ddlEduLevelEdu, "SELECT EduLevelCode, EduLevelName From MsEduLevel", True, "EduLevelCode", "EduLevelName", ViewState("DBConnection").ToString)
                FillCombo(ddlBank, "SELECT BankCode, BankName From MsBank", True, "BankCode", "BankName", ViewState("DBConnection").ToString)
                'FillCombo(ddlDepartment, "Select Dept_Code, Dept_Name FROM VMsSubsection Where Sub_Section_Code = " + QuotedStr(ddlSubSection.SelectedValue), True, "Dept_Code", "Dept_Name", ViewState("DBConnection").ToString)
                FillCombo(ddlDepartment, "Select Dept_Code, Dept_Name FROM VMsDepartment", True, "Dept_Code", "Dept_Name", ViewState("DBConnection").ToString)

                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnadd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnadddt.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddExp.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnprint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
                tbWeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbHeight.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbGPA.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbGraduated.Attributes.Add("OnKeyDown", "return PressNumeric();")
                tbsalaryExp.Attributes.Add("OnKeyDown", "return PressNumeric();")
                'tbsalaryExp.Attributes.Add("OnBlur", "setformatDt();")
                'btnaddb.Visible = True
                'btnaddb.Visible = Gridb.Rows.Count = 0
            End If
            lbStatus.Text = ""

            If Not Session("StrResult") Is Nothing Then
                BindData(2, Session("StrResult"))
                Session("StrResult") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnTraining" Then
                    tbTrainingCode.Text = Session("Result")(0).ToString
                    BindToText(tbTrainingName, Session("Result")(1).ToString)
                    Exit Sub
                End If
                If ViewState("Sender") = "btnCandidate" Then
                    'Type_Card, ID_Card, Job_Title_Code, Emp_Status_Code, Email"

                    tbCandidate.Text = Session("Result")(0).ToString
                    BindToText(tbName, Session("Result")(1).ToString)
                    ddlGender.SelectedValue = Session("Result")(2).ToString
                    BindToText(tbBirthPlace, Session("Result")(3).ToString)
                    BindToDate(tbBirthDate, Session("Result")(4).ToString)
                    BindToText(tbHeight, Session("Result")(5).ToString)
                    BindToText(tbWeight, Session("Result")(6).ToString)
                    BindToText(tbResAddr1, Session("Result")(7).ToString)
                    BindToText(tbResAddr2, Session("Result")(8).ToString)
                    BindToText(tbResZipCode, Session("Result")(9).ToString)
                    BindToText(tbResPhone, Session("Result")(10).ToString)
                    BindToText(tbHP, Session("Result")(11).ToString)
                    BindToText(tbOriAddr1, Session("Result")(12).ToString)
                    BindToText(tbOriAddr2, Session("Result")(13).ToString)
                    BindToText(tbOriZipCode, Session("Result")(14).ToString)
                    BindToText(tbOriPhone, Session("Result")(15).ToString)
                    BindToDropList(ddlReligion, Session("Result")(16).ToString)
                    BindToDropList(ddlMaritalSt, Session("Result")(17).ToString)
                    BindToDropList(ddlTypeCard, Session("Result")(18).ToString)
                    BindToText(tbIDCard, Session("Result")(19).ToString)
                    BindToDropList(ddlJobTitle, Session("Result")(20).ToString)
                    BindToDropList(ddlEmpStatus, Session("Result")(21).ToString)
                    BindToText(tbEmail, Session("Result")(22).ToString)
                    BindToDropList(ddlResCity, Session("Result")(23).ToString)
                    BindToDropList(ddlOriCity, Session("Result")(24).ToString)
                    'tbCandidate_TextChanged(Nothing, Nothing)
                    Exit Sub
                End If
                If ViewState("Sender") = "btnGetDt" Then
                    Dim Row, AllRow As DataRow()
                    Dim drResult As DataRow

                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = MessageDlg("Session is empty")
                        Exit Sub
                    End If
                    For Each drResult In Session("Result").Rows
                        AllRow = ViewState("Dt").Select()
                        Row = ViewState("Dt").select("ItemNo = " + QuotedStr(drResult("ItemNo")))
                        If Row.Length = 0 Then
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            If AllRow.Length = 0 Then
                                dr("ItemNo") = "1"
                            Else
                                dr("ItemNo") = (CInt(AllRow(AllRow.Length - 1)("ItemNo").ToString) + 1).ToString
                            End If
                            dr("LanguageName") = drResult("LanguageName")
                            dr("GradeRead") = drResult("GradeRead")
                            dr("GradeWrite") = drResult("GradeWrite")
                            dr("GradeSpeak") = drResult("DeliveryAddr1")
                            dr("Exam") = drResult("Exam")
                            dr("GPD") = drResult("GPD")
                            ViewState("Dt").Rows.Add(dr)
                        Else
                            'edit
                            Row(0).BeginEdit()
                            Row(0)("LanguageName") = drResult("LanguageName")
                            Row(0)("GradeRead") = drResult("GradeRead")
                            Row(0)("GradeWrite") = drResult("GradeWrite")
                            'Row(0)("City") = DBNull.Value
                            Row(0)("GradeSpeak") = drResult("GradeSpeak")
                            Row(0)("Exam") = drResult("Exam")
                            Row(0)("GPD") = drResult("GPD")
                            Row(0).EndEdit()
                        End If
                    Next
                    GridDt.DataSource = ViewState("Dt")
                    GridDt.DataBind()
                    Session("ColumnName") = Nothing
                End If
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

    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Insert" Then
                If ViewState("MenuLevel").Rows(0)("FgInsert") = "N" Then
                    lbStatus.Text = MessageDlg("You are not authorized to edit record. Please contact administrator")
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lbStatus.Text = MessageDlg("You are not authorized to edit record. Please contact administrator")
                    Return False
                    Exit Function
                End If
            End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbStatus.Text = MessageDlg("You are not authorized to delete record. Please contact administrator")
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function
    Private Sub BindData(ByVal page As Integer, Optional ByVal AdvanceFilter As String = "")
        Dim DS As DataSet
        Dim StrFilter As String
        Dim DV As DataView
        Try
            If page = 1 Then
                GridView1.PageIndex = 0
            End If
            If AdvanceFilter.Length > 1 Then
                StrFilter = " WHERE " + AdvanceFilter
            Else
                If tbFilter.Text.Trim.Length > 0 Then
                    If tbfilter2.Text.Trim.Length > 0 And pnlSearch.Visible Then
                        If ddlField.SelectedValue = "Gender" Then
                            StrFilter = " WHERE " + ddlField.SelectedValue + " = '" + tbFilter.Text + "' " + _
                            ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " Like '%" + tbfilter2.Text + "%'"
                        ElseIf ddlField2.SelectedValue = "Gender" Then
                            StrFilter = " WHERE " + ddlField.SelectedValue + " Like '%" + tbFilter.Text + "%' " + _
                            ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " = '" + tbfilter2.Text + "'"
                        ElseIf ddlField.SelectedValue = "Gender" And ddlField2.SelectedValue = "Gender" Then
                            StrFilter = " WHERE " + ddlField.SelectedValue + " = '" + tbFilter.Text + "' " + _
                            ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " = '" + tbfilter2.Text + "'"
                        Else
                            StrFilter = " WHERE " + ddlField.SelectedValue + " like '%" + tbFilter.Text + "%' " + _
                            ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " like '%" + tbfilter2.Text + "%'"
                        End If
                Else
                        If ddlField.SelectedValue = "Gender" Then
                            StrFilter = " WHERE A.EmpNumb IS NOT NULL AND " + ddlField.SelectedValue + " = '" + tbFilter.Text + "'"
                        Else
                            StrFilter = " WHERE A.EmpNumb IS NOT NULL AND " + ddlField.SelectedValue + " like '%" + tbFilter.Text + "%'"
                        End If

                End If
                Else
                    StrFilter = " Where A.EmpNumb IS NOT NULL "
                End If
            End If
            DS = SQLExecuteQuery("SELECT  A.*, L.ReligionName AS ReligionName, B.JobTtlName As Job_Title, C.JobLvlName As Job_Level, G.EmpStatusName As Emp_Status_Name, G.CanLeave, F.WorkPlaceName as Work_Place, " + _
                                " J.Dept_Code, J.Dept_Name, G.FgPermanent" + _
                                " FROM MsEmployee A LEFT OUTER JOIN  " + _
                                " MsJobTitle B ON A.JobTitle = B.JobTtlCode LEFT OUTER JOIN " + _
                                " MsJobLevel C ON A.JobLevel = C.JobLvlCode LEFT OUTER JOIN " + _
                                " MsWorkPlace F ON A.WorkPlace = F.WorkPlaceCode LEFT OUTER JOIN  " + _
                                " MsEmpStatus G ON A.EmpStatus = G.EmpStatusCode LEFT OUTER JOIN  " + _
                                " VMsDepartment J ON A.Department = J.Dept_Code LEFT OUTER JOIN  " + _
                                " MsMethod D ON A.MethodSalary = D.MethodCode LEFT OUTER JOIN  " + _
                                " MsReligion L ON A.Religion = L.ReligionCode " + StrFilter, ViewState("DBConnection").ToString)

            
            If DS.Tables(0).Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
            End If

            DV = DS.Tables(0).DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "EmpNumb ASC"
            End If

            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub ModifyInput(ByVal State As Boolean)
        Try
            tbCode.Enabled = State
            tbCandidate.Enabled = State
            ddlReligion.Enabled = State
            tbName.Enabled = State
            tbStartDate.Enabled = State
            ddlGender.Enabled = State
            ddlBloodType.Enabled = State
            tbStartDate.Enabled = State
            tbBirthDate.Enabled = State
            tbBirthPlace.Enabled = State
            tbEndDate.Enabled = State
            ddlTypeCard.Enabled = State
            tbIDCard.Enabled = State
            tbMaritalDocNo.Enabled = State
            tbDateMarital.Enabled = State
            tbTribe.Enabled = State
            tbWeight.Enabled = State
            tbHeight.Enabled = State
            tbPhoneContact.Enabled = State
            tbEmail.Enabled = State
            ddlMaritalSt.Enabled = State
            ddlMaritalTax.Enabled = State
            'ddlActive.Enabled = State
            ddlSalaryType.Enabled = State
            tbNPWP.Enabled = State
            tbCard.Enabled = State
            ddlJobTitle.Enabled = State
            ddlJobLevel.Enabled = State
            ddlEmpStatus.Enabled = State
            ddlWorkPlace.Enabled = State
            ddlDepartment.Enabled = State
            ddlTKA.Enabled = State
            ddlFgJamsosTek.Enabled = State
            ddlResCity.Enabled = State
            ddlOriCity.Enabled = State
            tbSKNo.Enabled = State
            tbAKDHK.Enabled = State
            tbPinBB.Enabled = State
            tbjamsostek.Enabled = State
            tbResAddr1.Enabled = State
            tbResAddr2.Enabled = State
            tbResZipCode.Enabled = State
            tbOriAddr1.Enabled = State
            tbOriAddr2.Enabled = State
            tbOriZipCode.Enabled = State
            tbResPhone.Enabled = State
            tbOriPhone.Enabled = State
            tbJamsostekDate.Enabled = State
            tbEndContract.Enabled = State
            ddlResAddrStatus.Enabled = State
            ddlStatus.Enabled = State
            tbLastNo.Enabled = State
            ddlMethod.Enabled = State
            'PnlLimit.Visible = False
            btnSaveHd.Enabled = State
            btnCancelHd.Enabled = State
            btnCandidate.Enabled = State
            'ddlSubSection.Enabled = State
            'ddlsection.Enabled = State
        Catch ex As Exception
            Throw New Exception("Modify Input Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbCode.Enabled = State
            tbCandidate.Enabled = State
            ddlReligion.Enabled = State
            tbName.Enabled = State
            tbStartDate.Enabled = State
            ddlGender.Enabled = State
            tbStartDate.Enabled = State
            tbBirthDate.Enabled = State
            tbBirthPlace.Enabled = State
            ddlTypeCard.Enabled = State
            tbIDCard.Enabled = State
            tbMaritalDocNo.Enabled = State
            tbDateMarital.Enabled = State
            tbTribe.Enabled = State
            tbEndDate.Enabled = State
            tbWeight.Enabled = State
            tbHeight.Enabled = State
            tbPhoneContact.Enabled = State
            tbEmail.Enabled = State
            ddlMaritalSt.Enabled = State
            ddlMaritalTax.Enabled = State
            'ddlActive.Enabled = State
            ddlSalaryType.Enabled = State
            tbNPWP.Enabled = State
            tbCard.Enabled = State
            ddlJobTitle.Enabled = State
            ddlJobLevel.Enabled = State
            ddlEmpStatus.Enabled = State
            ddlWorkPlace.Enabled = State
            ddlDepartment.Enabled = State
            ddlTKA.Enabled = State
            ddlFgJamsosTek.Enabled = State
            ddlResCity.Enabled = State
            ddlOriCity.Enabled = State
            tbSKNo.Enabled = State
            tbAKDHK.Enabled = State
            tbPinBB.Enabled = State
            tbjamsostek.Enabled = State
            tbResAddr1.Enabled = State
            tbResAddr2.Enabled = State
            tbResZipCode.Enabled = State
            tbOriAddr1.Enabled = State
            tbOriAddr2.Enabled = State
            tbOriZipCode.Enabled = State
            tbResPhone.Enabled = State
            tbOriPhone.Enabled = State
            tbJamsostekDate.Enabled = State
            tbEndContract.Enabled = State
            ddlResAddrStatus.Enabled = State
            ddlStatus.Enabled = State
            tbLastNo.Enabled = State
            btnCandidate.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Function GetString(ByVal Nmbr As String) As String
        Return "SELECT * FROM MsEmployee  WHERE EmpNumb = '" + Nmbr + "'"
    End Function

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT EmpNumb, ItemNo, LanguageName, GradeRead, GradeWrite, GradeSpeak, Exam, GPD FROM MsEmpLanguage WHERE EmpNumb = '" + Nmbr + "'"
    End Function

    Private Function GetStringDtFamily(ByVal Nmbr As String) As String
        Return "SELECT EmpNumb, ItemNo, FamilyType, FamilyName, Occupation, Address1, Address2, FgMedical, Phone, BirthPlace, BirthDate, Gender, EduLevel FROM MsEmpFamily WHERE EmpNumb = '" + Nmbr + "'"
    End Function
    Private Function GetStringDtSkill(ByVal Nmbr As String) As String
        Return "SELECT A.* FROM MsEmpSkill A WHERE A.EmpNumb = '" + Nmbr + "'"
    End Function

    Private Function GetStringDtEdu(ByVal Nmbr As String) As String
        Return "SELECT A.* FROM MsEmpEdu A WHERE A.EmpNumb = '" + Nmbr + "'"
    End Function

    Private Function GetStringDtExp(ByVal Nmbr As String) As String
        Return "SELECT ItemNo, CompanyName, CompanyBusiness, WorkPeriod, Address1, Address2, CompanyCity, Phone,JobTitle, Department, JobResponsibility, Currency, LastSalary, PHKReason FROM MsEmpExp WHERE EmpNumb = '" + Nmbr + "'"
    End Function
    Private Function GetStringDtTraining(ByVal Nmbr As String) As String
        Return "SELECT EmpNumb, ItemNo, TrainingCode, TrainingName, TrainingPlace, TrainingPeriod, Institution, Location, TutorName, Nilai, CostType, FgCertificate, Certificate, UserId, UserDate FROM MsEmpTraining WHERE EmpNumb = '" + Nmbr + "'"
    End Function
    Private Function GetStringDtPrestasi(ByVal Nmbr As String) As String
        Return "SELECT A.* FROM MsEmpPrestasi A WHERE A.EmpNumb = '" + Nmbr + "'"
    End Function
    Private Function GetStringDtBank(ByVal Nmbr As String) As String
        Return "SELECT A.* FROM MsEmpBank A WHERE A.EmpNumb = '" + Nmbr + "'"
    End Function
    Private Function GetStringDtEmergency(ByVal Nmbr As String) As String
        Return "SELECT A.* FROM MsEmpEmergency A WHERE A.EmpNumb = '" + Nmbr + "'"
    End Function
    Private Function GetStringDtSociety(ByVal Nmbr As String) As String
        Return "SELECT A.* FROM MsEmpSociety A WHERE A.EmpNumb = '" + Nmbr + "'"
    End Function

    Private Function GetStringDtHobby(ByVal Nmbr As String) As String
        Return "SELECT A.* FROM MsEmpHobby A WHERE A.EmpNumb = '" + Nmbr + "'"
    End Function
    Private Function GetStringDtMemo(ByVal Nmbr As String) As String
        Return "SELECT A.* FROM MsEmpMemo A WHERE A.EmpNumb = '" + Nmbr + "'"
    End Function

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt") = dt


            GridDt.DataSource = dt
            GridDt.DataBind()
            BindGridDt(dt, GridDt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        Try
            pnlView.Visible = False
            PnlHd.Visible = True
            pnlDt.Visible = False
            PnlExp.Visible = False
            PnlSkill.Visible = False
            PnlP.Visible = False
            btnadddt.Visible = False
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelDt.Click
        Try
            pnlEditDt.Visible = False
            pnlDt.Visible = True
            'EnableHd(GridDt.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveHd()
        Dim SQLString, Endcontract, Enddate As String
        Dim path, namafile As String
        'lbGroup.Text = "Employee"
        Try
            If CekHd() = False Then
                Exit Sub
            End If
            'If tbEndContract.SelectedValue = "" Then
            '    Endcontract = ""
            'Else : Endcontract = Format(tbEndContract.SelectedValue, "yyyy-MM-dd").Replace("'1900-01-01'", "NULL")

            '    If tbEndDate.SelectedValue = "" Then
            '        Enddate = ""
            '    Else : Enddate = Format(tbEndDate.SelectedValue, "yyyy-MM-dd").Replace("'1900-01-01'", "NULL")

            ' Endcontract = Format(tbEndContract.SelectedValue, "yyyy-MM-dd").Replace("'1900-01-01'", "NULL")
            '  Enddate = Format(tbEndDate.SelectedValue, "yyyy-MM-dd").Replace("'1900-01-01'", "NULL")

            If ViewState("StateHd") = "Insert" Then
                If SQLExecuteScalar("SELECT Emp_No From V_MsEmployeeMain WHERE Emp_No = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Employee  " + QuotedStr(tbCode.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmployee (EmpNumb, EmpName, ApplicantNo, Gender, BirthPlace, BirthDate, HireDate, EndDate, Religion, Tribe, ResAddr1, ResAddr2," + _
                " ResPostCode, ResCity, ResPhone, ResAddrStatus, OriAddr1, OriAddr2, OriPostCode, OriCity, OriPhone, TypeCard, IDCard, " + _
                " BloodType, AbsenceCard, Weight, Height, HandPhone, Email, PINBB, LastCertificateNo, NPWP, MaritalTax, MaritalSt, " + _
                " MaritalDate, MaritalDocNo, FgJamsosTek, JamSosTekNo, JamSosTekDate, AKDHKNo, SalaryType, FgTKA, TKAStatus, FgActive, " + _
                " UserID, UserDate, JobLevel, Department, JobTitle, EmpStatus, WorkPlace, SKNo, EndDateContract, MethodSalary) " + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + "," + QuotedStr(tbCandidate.Text) + "," + QuotedStr(ddlGender.SelectedValue) + "," + QuotedStr(tbBirthPlace.Text) + ", " + QuotedStr(Format(tbBirthDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ddlReligion.SelectedValue) + ", " + _
               QuotedStr(tbTribe.Text) + ", " + QuotedStr(tbResAddr1.Text) + ", " + QuotedStr(tbResAddr2.Text) + ", " + QuotedStr(tbResZipCode.Text) + ", " + QuotedStr(ddlResCity.SelectedValue) + ", " + _
               QuotedStr(tbResPhone.Text) + ", " + QuotedStr(ddlResAddrStatus.SelectedValue) + ", " + QuotedStr(tbOriAddr1.Text) + ", " + QuotedStr(tbOriAddr2.Text) + ", " + QuotedStr(tbOriZipCode.Text) + "," + QuotedStr(ddlOriCity.SelectedValue) + ", " + QuotedStr(tbOriPhone.Text) + ", " + _
               QuotedStr(ddlTypeCard.SelectedValue) + ", " + QuotedStr(tbIDCard.Text) + ", " + _
               QuotedStr(ddlBloodType.SelectedValue) + ", " + QuotedStr(tbCard.Text) + ", " + QuotedStr(tbWeight.Text.Replace(",", "")) + ", " + tbHeight.Text.Replace(",", "") + ", " + _
               QuotedStr(tbPhoneContact.Text) + ", " + QuotedStr(tbEmail.Text) + ", " + QuotedStr(tbPinBB.Text) + ", " + QuotedStr(tbCertificate.Text) + ", " + _
               QuotedStr(tbNPWP.Text) + ", " + QuotedStr(ddlMaritalTax.SelectedValue) + ", " + QuotedStr(ddlMaritalSt.SelectedValue) + ", " + _
               QuotedStr(Format(tbDateMarital.SelectedValue, "yyyy-MM-dd")) + ", " + _
               QuotedStr(tbMaritalDocNo.Text) + ", " + QuotedStr(ddlFgJamsosTek.SelectedValue) + ", " + QuotedStr(tbjamsostek.Text) + ", " + QuotedStr(Format(tbJamsostekDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbAKDHK.Text) + ", " + _
               QuotedStr(ddlSalaryType.Text) + ", " + QuotedStr(ddlTKA.SelectedValue) + ", " + QuotedStr(ddlStatus.SelectedValue) + ", " + QuotedStr(ddlActive.SelectedValue) + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", getDate() , " + _
               QuotedStr(ddlJobLevel.SelectedValue) + ", " + QuotedStr(ddlDepartment.SelectedValue) + ", " + _
               QuotedStr(ddlJobTitle.SelectedValue) + ", " + QuotedStr(ddlEmpStatus.SelectedValue) + ", " + _
               QuotedStr(ddlWorkPlace.SelectedValue) + ", " + QuotedStr(tbSKNo.Text) + ", " + _
               QuotedStr(Format(tbEndContract.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ddlMethod.SelectedValue) + ""
            Else
                SQLString = "UPDATE MsEmployee SET EmpName = " + QuotedStr(tbName.Text) + ", " + _
                " ApplicantNo = " + QuotedStr(tbCandidate.Text) + ", Gender= " + QuotedStr(ddlGender.SelectedValue) + ", " + _
                " BirthPlace = " + QuotedStr(tbBirthPlace.Text) + ", BirthDate = " + QuotedStr(Format(tbBirthDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                " HireDate = " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", EndDate = " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd").Replace("'1900-01-01'", "NULL")) + ", " + _
                " Religion = " + QuotedStr(ddlReligion.SelectedValue.Replace("&nbsp;", "")) + ", Tribe = " + QuotedStr(tbTribe.Text) + ", " + _
                " ResAddr1 = " + QuotedStr(tbResAddr1.Text) + ", ResAddr2 = " + QuotedStr(tbResAddr2.Text) + ", " + _
                " ResPostCode = " + QuotedStr(tbResZipCode.Text) + ", ResCity = " + QuotedStr(ddlResCity.SelectedValue.Replace("&nbsp;", "")) + ", " + _
                " ResPhone = " + QuotedStr(tbResPhone.Text) + ", " + _
                " ResAddrStatus = " + QuotedStr(ddlResAddrStatus.SelectedValue.Replace("&nbsp;", "")) + ", OriAddr1 = " + QuotedStr(tbOriAddr1.Text) + ", " + _
                " OriAddr2 = " + QuotedStr(tbOriAddr2.Text) + ", OriPostCode = " + QuotedStr(tbOriZipCode.Text) + ", " + _
                " OriCity = " + QuotedStr(ddlOriCity.SelectedValue.Replace("&nbsp;", "")) + ", OriPhone = " + QuotedStr(tbOriPhone.Text) + ", " + _
                " TypeCard = " + QuotedStr(ddlTypeCard.SelectedValue.Replace("&nbsp;", "")) + ", IDCard = " + QuotedStr(tbIDCard.Text) + ", " + _
                " BloodType = " + QuotedStr(ddlBloodType.SelectedValue.Replace("&nbsp;", "")) + ", AbsenceCard = " + QuotedStr(tbCard.Text) + ", " + _
                " Weight = " + QuotedStr(tbWeight.Text.Replace(",", "")) + ", Height = " + tbHeight.Text.Replace(",", "") + ", " + _
                " HandPhone = " + QuotedStr(tbPhoneContact.Text) + ", Email = " + QuotedStr(tbEmail.Text) + ", " + _
                " PINBB = " + QuotedStr(tbPinBB.Text) + ", LastCertificateNo = " + QuotedStr(tbLastNo.Text) + ", " + _
                " NPWP = " + QuotedStr(tbNPWP.Text) + ", MaritalTax = " + QuotedStr(ddlMaritalTax.SelectedValue.Replace("&nbsp;", "")) + ", " + _
                " MaritalSt = " + QuotedStr(ddlMaritalSt.SelectedValue.Replace("&nbsp;", "")) + ", UserID = " + QuotedStr(ViewState("UserId").ToString) + ", " + _
                " UserDate = getdate(), " + _
                " JobLevel = " + QuotedStr(ddlJobLevel.SelectedValue.Replace("&nbsp;", "")) + ", Department = " + QuotedStr(ddlDepartment.SelectedValue.Replace("&nbsp;", "")) + ", " + _
                " JobTitle = " + QuotedStr(ddlJobTitle.SelectedValue.Replace("&nbsp;", "")) + ", EmpStatus = " + QuotedStr(ddlEmpStatus.SelectedValue.Replace("&nbsp;", "")) + ", " + _
                " WorkPlace = " + QuotedStr(ddlWorkPlace.SelectedValue.Replace("&nbsp;", "")) + ", SKNo = " + QuotedStr(tbSKNo.Text) + ", " + _
                " EndDateContract = " + QuotedStr(Format(tbEndContract.SelectedValue, "yyyy-MM-dd").Replace("'1900-01-01'", "NULL")) + ", MaritalDate = " + QuotedStr(Format(tbDateMarital.SelectedValue, "yyyy-MM-dd")) + ", " + _
                " MaritalDocNo = " + QuotedStr(tbMaritalDocNo.Text) + ", FgJamsosTek = " + QuotedStr(ddlFgJamsosTek.SelectedValue.Replace("&nbsp;", "")) + ", " + _
                " JamSosTekNo = " + QuotedStr(tbjamsostek.Text) + ", JamSosTekDate = " + QuotedStr(Format(tbJamsostekDate.SelectedValue, "yyyy-MM-dd")) + ", " + _
                " AKDHKNo = " + QuotedStr(tbAKDHK.Text) + ", SalaryType = " + QuotedStr(ddlSalaryType.SelectedValue.Replace("&nbsp;", "")) + ", " + _
                " FgTKA = " + QuotedStr(ddlTKA.SelectedValue.Replace("&nbsp;", "")) + ", TKAStatus = " + QuotedStr(ddlStatus.SelectedValue.Replace("&nbsp;", "")) + ", " + _
                " FgActive = " + QuotedStr(ddlActive.SelectedValue) + ", " + _
                " MethodSalary = " + QuotedStr(ddlMethod.SelectedValue) + _
                " WHERE EmpNumb = '" + tbCode.Text + "'"
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlView.Visible = False
            PnlHd.Visible = True
            BindData(1)
        Catch ex As Exception
            Throw New Exception("Save Hd Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub DeleteHd(ByVal Nmbr As String)
        Dim SQLString As String
        Try
            SQLString = "UPDATE MsEmployee SET FgActive = 'N' where EmpNumb = '" & Nmbr & "'"
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Delete Hd Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Try
            ViewState("StateHd") = "Insert"
            'newTrans()
            ClearHd()

            PnlHd.Visible = False
            pnlView.Visible = True
            ModifyInput(True)
            MultiView1.Visible = True
            MultiView1.ActiveViewIndex = 0
            Menu2.Visible = True
            Menu2.Items.Item(0).Selected = True
            Menu1.Visible = True
            tbCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ClearHd()
            cleardt()
            ViewState("Dt") = SQLExecuteQuery("Select A.* from MsEmpLanguage A WHERE EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
            ViewState("DtF") = SQLExecuteQuery("Select A.* from MsEmpFamily A WHERE EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            Gridf.DataSource = ViewState("DtF")
            Gridf.DataBind()
            ViewState("DtS") = SQLExecuteQuery("Select A.* from MsEmpSkill A WHERE EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            GridS.DataSource = ViewState("DtS")
            GridS.DataBind()
            ViewState("DtEdu") = SQLExecuteQuery("Select A.* from MsEmpEdu A WHERE EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            GridEdu.DataSource = ViewState("DtEdu")
            GridEdu.DataBind()
            ViewState("DtExp") = SQLExecuteQuery("Select A.* from MsEmpExp A WHERE A.EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            GridExp.DataSource = ViewState("DtExp")
            GridExp.DataBind()
            ViewState("DtT") = SQLExecuteQuery("Select A.* from MsEmpTraining A WHERE A.EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            Gridt.DataSource = ViewState("DtT")
            Gridt.DataBind()
            ViewState("DtP") = SQLExecuteQuery("SELECT A.EmpNumb, A.Tahun, A.Prestasi FROM MsEmpPrestasi A WHERE A.EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            GridP.DataSource = ViewState("DtP")
            GridP.DataBind()
            ViewState("DtB") = SQLExecuteQuery("SELECT A.* FROM MsEmpBank A WHERE A.EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            Gridb.DataSource = ViewState("DtB")
            Gridb.DataBind()
            ViewState("DtEme") = SQLExecuteQuery("SELECT A.* FROM MsEmpEmergency A WHERE A.EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            Grideme.DataSource = ViewState("DtEme")
            Grideme.DataBind()
            ViewState("DtSO") = SQLExecuteQuery("SELECT A.* FROM MsEmpSociety A WHERE A.EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            Gridso.DataSource = ViewState("DtSO")
            Gridso.DataBind()
            ViewState("DtH") = SQLExecuteQuery("SELECT A.* FROM MsEmpHobby A WHERE A.EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            Gridh.DataSource = ViewState("DtH")
            Gridh.DataBind()
            ViewState("DtM") = SQLExecuteQuery("SELECT A.* FROM MsEmpMemo A WHERE A.EmpNumb = ''", ViewState("DBConnection").ToString).Tables(0)
            GridM.DataSource = ViewState("DtM")
            GridM.DataBind()
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbName.Text = ""
            ddlGender.SelectedIndex = 1
            tbCandidate.Text = ""
            ddlBloodType.SelectedIndex = 0
            tbWeight.Text = 0
            tbBirthPlace.Text = ""
            tbHeight.Text = 0
            tbBirthDate.SelectedValue = ""
            ddlMaritalSt.SelectedIndex = 0
            ddlReligion.SelectedIndex = 0
            ddlMaritalTax.SelectedValue = ""
            tbTribe.Text = ""
            tbMaritalDocNo.Text = ""
            tbCard.Text = ""
            tbDateMarital.SelectedValue = ""
            ddlTypeCard.SelectedIndex = 0
            ddlActive.SelectedIndex = 0
            tbIDCard.Text = ""
            tbSKNo.Text = ""
            tbStartDate.SelectedValue = ""
            tbEndDate.SelectedValue = ""
            ddlJobTitle.SelectedValue = ""
            ddlJobLevel.SelectedValue = ""
            ddlDepartment.SelectedValue = ""
            ddlEmpStatus.SelectedValue = ""
            tbEndContract.SelectedValue = ""
            ddlWorkPlace.SelectedValue = ""
            ddlSalaryType.SelectedIndex = 1
            tbPhoneContact.Text = ""
            tbEmail.Text = ""
            tbPinBB.Text = ""
            tbResAddr1.Text = ""
            tbOriAddr1.Text = ""
            tbResAddr2.Text = ""
            tbOriAddr2.Text = ""
            tbResZipCode.Text = ""
            tbOriZipCode.Text = ""
            ddlResCity.SelectedValue = ""
            ddlOriCity.SelectedValue = ""
            tbResPhone.Text = ""
            tbOriPhone.Text = ""
            ddlResAddrStatus.SelectedValue = ""
            tbNPWP.Text = ""
            ddlFgJamsosTek.SelectedIndex = 1
            ddlMethod.SelectedIndex = 0
            tbjamsostek.Text = ""
            tbJamsostekDate.SelectedValue = ""
            tbAKDHK.Text = ""
            ddlTKA.SelectedIndex = 1
            ddlStatus.SelectedValue = ""
            ddlDepartment.SelectedValue = ""
            tbLastNo.Text = ""
            btnSaveHd.Visible = True
            btnCancelHd.Visible = True
            btnReset.Visible = True
            tbjamsostek.Enabled = False
            tbJamsostekDate.Enabled = False
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub cleardt()
        Try
            lbItemNo.Text = ""
            tbLanguage.Text = ""
            ddlRead.SelectedIndex = 0
            ddlWrite.SelectedIndex = 0
            ddlSpeak.SelectedIndex = 0
            tbexam.Text = ""
            tbGPD.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub cleardtP()
        Try
            tbtahunP.Text = ""
            tbprestasi.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt P Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnexpand.Click
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

    Function CekHd() As Boolean
        Try
            If tbCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("NIK must have value")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Employee Name must have value")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                tbName.Focus()
                Return False
            End If
            If Not IsNumeric(tbWeight.Text) Then
                lbStatus.Text = MessageDlg("Weight must be numeric type.")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                tbWeight.Focus()
                Exit Function
            End If
            If Not IsNumeric(tbHeight.Text) Then
                lbStatus.Text = MessageDlg("Height must be numeric type.")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                tbHeight.Focus()
                Exit Function
            End If
            If tbBirthDate.IsNull Then
                lbStatus.Text = MessageDlg("Birth Date must have value")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                tbBirthDate.Focus()
                Return False
            End If
            If ddlReligion.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Religion must have value")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                ddlReligion.Focus()
                Return False
            End If
            If tbCard.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Absence Card No must have value")
                Menu2.Items.Item(0).Selected = True
                tbCard.Focus()
                Return False
            End If
            If tbIDCard.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("ID Card must have value")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                tbIDCard.Focus()
                Return False
            End If
            If ddlMaritalSt.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Marital Status must have value")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                ddlMaritalSt.Focus()
                Return False
            End If
            If ddlMaritalTax.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Marital Tax must have value")
                MultiView1.ActiveViewIndex = 0
                Menu2.Items.Item(0).Selected = True
                ddlMaritalTax.Focus()
                Return False
            End If
            If tbStartDate.IsNull Then
                lbStatus.Text = MessageDlg("Start Date must have value")
                MultiView1.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                tbStartDate.Focus()
                Return False
            End If
            If ddlJobTitle.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Job Title must have value")
                MultiView1.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                ddlJobTitle.Focus()
                Return False
            End If
            If ddlJobLevel.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Job Level must have value")
                MultiView1.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                ddlJobLevel.Focus()
                Return False
            End If
            If ddlDepartment.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Department must have value")
                MultiView1.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                ddlDepartment.Focus()
                Return False
            End If
            If ddlEmpStatus.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Employee Status must have value")
                MultiView1.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                ddlEmpStatus.Focus()
                Return False
            End If
            If ddlWorkPlace.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Work Place must have value")
                MultiView1.ActiveViewIndex = 1
                Menu2.Items.Item(1).Selected = True
                ddlWorkPlace.Focus()
                Return False
            End If
            If ddlResCity.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Res City must have value")
                MultiView1.ActiveViewIndex = 2
                Menu2.Items.Item(2).Selected = True
                ddlResCity.Focus()
                Return False
            End If
            If ddlOriCity.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Ori City must have value")
                MultiView1.ActiveViewIndex = 2
                Menu2.Items.Item(2).Selected = True
                ddlOriCity.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(ByVal Dr As DataRow) As Boolean
        Try
            If Dr.RowState = DataRowState.Deleted Then
                Return True
            End If
            If Dr("LanguageName").ToString.Trim = "" Then
                lbStatus.Text = MessageDlg("Language Name Must Have Value")
                tbLanguage.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Function CekEdu() As Boolean
        Try
            If ddlEduLevelEdu.ToString.Trim = "" Then
                lbStatus.Text = MessageDlg("Education Level Must Have Value")
                ddlEduLevelEdu.Focus()
                Return False
            End If
            If tbSchollName.ToString.Trim = "" Then
                lbStatus.Text = MessageDlg("School Name Must Have Value")
                tbSchollName.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In GridView1.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            lbStatus.Text = "cb Hd Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData(2)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim DDL As DropDownList
        Dim GVR As GridViewRow = Nothing
        Dim index As Integer
        Try

            If Not (e.CommandName = "Sort" Or e.CommandName = "Page") Then
                index = Convert.ToInt32(e.CommandArgument)
                GVR = GridView1.Rows(index)
            End If

            If e.CommandName = "Go" Then
                DDL = GridView1.Rows(index).FindControl("ddl")
                If DDL.SelectedValue = "View" Then

                    PnlHd.Visible = False
                    pnlView.Visible = True
                    pnlDt.Visible = True
                    PnlExp.Visible = True
                    PnlSkill.Visible = True
                    PnlP.Visible = True
                    btnadddt.Visible = True
                    pnlEditDt.Visible = False
                    ' PnlEditExp.Visible = False
                    PnlEditExp.Visible = False
                    PnlEditPrestasi.Visible = False
                    ViewState("EmpNumb") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    BindDataDt(ViewState("EmpNumb"))
                    Gridf.PageIndex = 0
                    BindDataDtFamily(ViewState("EmpNumb"))
                    GridS.PageIndex = 0
                    BindDataDtSkill(ViewState("EmpNumb"))
                    GridEdu.PageIndex = 0
                    BindDataDtEdu(ViewState("EmpNumb"))
                    GridExp.PageIndex = 0
                    BindDataDtExp(ViewState("EmpNumb"))
                    Gridt.PageIndex = 0
                    BindDataDtTraining(ViewState("EmpNumb"))
                    Gridb.PageIndex = 0
                    BindDataDtBank(ViewState("EmpNumb"))
                    Grideme.PageIndex = 0
                    BindDataDtEme(ViewState("EmpNumb"))
                    Gridso.PageIndex = 0
                    BindDataDtSO(ViewState("EmpNumb"))
                    Gridh.PageIndex = 0
                    BindDataDtH(ViewState("EmpNumb"))
                    GridM.PageIndex = 0
                    BindDataDtM(ViewState("EmpNumb"))
                    GridP.PageIndex = 0
                    BindDataPrestasi(ViewState("EmpNumb"))
                    FillTextBoxHd(ViewState("EmpNumb"))
                    ViewState("StateHd") = "View"
                    MultiView1.Visible = True
                    Menu1.Visible = True
                    MultiView1.ActiveViewIndex = 0
                    Menu2.Items.Item(0).Selected = True
                    Menu1.Items.Item(0).Selected = True
                    ModifyInput(False)
                    btnSaveHd.Visible = False
                    btnCancelHd.Visible = False
                    btnReset.Visible = False

                    Dim image As String
                    image = SQLExecuteScalar("SELECT ImageFile FROM SAUploadImage WHERE ImageCode = '" + GVR.Cells(2).Text + "'", ViewState("DBConnection").ToString)
                    'lbStatus.Text = "'" + image + "'"
                    imgUpload.ImageUrl = "~/image/Employee/" + image + ""

                    'btnaddb.Visible = Gridb.Rows.Count = 0
                ElseIf DDL.SelectedValue = "Edit" Then
                    If GVR.Cells(27).Text = "N" Then
                        lbStatus.Text = MessageDlg("Employee Is Not Active, Can't Update")
                        Exit Sub
                    End If
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
                    PnlHd.Visible = False
                    pnlView.Visible = True
                    pnlDt.Visible = False
                    PnlExp.Visible = False
                    PnlSkill.Visible = False
                    PnlP.Visible = False
                    btnadddt.Visible = False
                    ViewState("EmpNumb") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    BindDataDt(ViewState("EmpNumb"))
                    Gridf.PageIndex = 0
                    BindDataDtFamily(ViewState("EmpNumb"))
                    GridS.PageIndex = 0
                    BindDataDtSkill(ViewState("EmpNumb"))
                    GridEdu.PageIndex = 0
                    BindDataDtEdu(ViewState("EmpNumb"))
                    GridExp.PageIndex = 0
                    BindDataDtExp(ViewState("EmpNumb"))
                    Gridt.PageIndex = 0
                    BindDataDtTraining(ViewState("EmpNumb"))
                    Gridb.PageIndex = 0
                    BindDataDtBank(ViewState("EmpNumb"))
                    Grideme.PageIndex = 0
                    BindDataDtEme(ViewState("EmpNumb"))
                    Gridso.PageIndex = 0
                    BindDataDtSO(ViewState("EmpNumb"))
                    Gridh.PageIndex = 0
                    BindDataDtH(ViewState("EmpNumb"))
                    GridM.PageIndex = 0
                    BindDataDtM(ViewState("EmpNumb"))
                    FillTextBoxHd(ViewState("EmpNumb"))
                    ViewState("StateHd") = "Edit"
                    ModifyInput(True)
                    MultiView1.ActiveViewIndex = 0
                    Menu2.Items.Item(0).Selected = True
                    EnableHd(True)
                    BindData(1)
                    Menu1.Visible = False
                    btnSaveHd.Visible = True
                    btnCancelHd.Visible = True
                    btnReset.Visible = False

                ElseIf DDL.SelectedValue = "Delete" Then
                    Try
                        If CheckMenuLevel("Delete") = False Then
                            Exit Sub
                        End If


                        ViewState("EmpNumb") = GVR.Cells(2).Text
                        DeleteHd(ViewState("EmpNumb"))
                        BindData(1)



                    Catch ex As Exception
                        lbStatus.Text = "DDL.SelectedValue = Delete Error : " + ex.ToString
                    End Try
                    'ElseIf DDL.SelectedValue = "Print" Then
                    '    Dim ReportGw As New ReportDocument
                    '   Dim Reportds As DataSet

                    '  Reportds = SQLExecuteQuery("EXEC S_GLFormJE " + QuotedStr(GVR.Cells(2).Text))

                    'ReportGw.SetParameterValue("@Nmbr", e.Item.Cells(2).Text)
                    ' ReportGw.Load(Server.MapPath("~\Rpt\FormJEntry.Rpt"))
                    'ReportGw.SetDataSource(Reportds.Tables(0))

                    'Session("Report") = ReportGw
                    'Response.Write("<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>")

                    'CrystalReportViewer1.ReportSource = ReportGw
                    'PnlHd.Visible = False
                    'pnlPrint.Visible = True
                ElseIf DDL.SelectedValue = "Copy New" Then
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                    PnlHd.Visible = False
                    pnlView.Visible = True
                    pnlDt.Visible = True
                    PnlExp.Visible = True
                    PnlSkill.Visible = True
                    PnlP.Visible = True
                    btnadddt.Visible = True
                    pnlEditDt.Visible = False
                    '  PnlEditExp.Visible = False
                    PnlEditExp.Visible = False
                    PnlEditPrestasi.Visible = False
                    ViewState("EmpNumb") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    BindDataDt(ViewState("EmpNumb"))
                    Gridf.PageIndex = 0
                    BindDataDtFamily(ViewState("EmpNumb"))
                    GridS.PageIndex = 0
                    BindDataDtSkill(ViewState("EmpNumb"))
                    GridEdu.PageIndex = 0
                    BindDataDtEdu(ViewState("EmpNumb"))
                    GridExp.PageIndex = 0
                    BindDataDtExp(ViewState("EmpNumb"))
                    Gridt.PageIndex = 0
                    BindDataDtTraining(ViewState("EmpNumb"))
                    GridP.PageIndex = 0
                    BindDataPrestasi(ViewState("EmpNumb"))
                    Gridb.PageIndex = 0
                    BindDataDtBank(ViewState("EmpNumb"))
                    Grideme.PageIndex = 0
                    BindDataDtEme(ViewState("EmpNumb"))
                    GridS.PageIndex = 0
                    BindDataDtSO(ViewState("EmpNumb"))
                    Gridh.PageIndex = 0
                    BindDataDtH(ViewState("EmpNumb"))
                    GridM.PageIndex = 0
                    BindDataDtM(ViewState("EmpNumb"))
                    FillTextBoxHd(ViewState("EmpNumb"))
                    ViewState("StateHd") = "Insert"
                    ModifyInput(True)
                    Menu1.Visible = True
                    Menu1.Items.Item(0).Selected = True
                    MultiView1.Visible = True
                    MultiView1.ActiveViewIndex = 0
                    btnSaveHd.Visible = True
                    btnCancelHd.Visible = True
                    btnReset.Visible = False
                ElseIf DDL.SelectedValue = "Photo" Then
                    Try
                        Dim paramgo As String
                        paramgo = GVR.Cells(2).Text + "|" + GVR.Cells(3).Text + "|Employee"
                        'If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenTransaction('TrUploadImage', '" + paramgo + "' );", True)
                        'End If
                        AttachScript("OpenTransaction('TrUploadImage', '" + Request.QueryString("KeyId") + "', '" + paramgo + "' );", Page, Me.GetType())
                    Catch ex As Exception
                        lbStatus.Text = "DDL.SelectedValue = Menu Error : " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print" Then
                    Try
                        'CekMenu = CheckMenuLevel("Print", ViewState("MenuLevel").Rows(0))
                        'If CekMenu <> "" Then
                        '    lbStatus.Text = CekMenu
                        '    Exit Sub
                        'End If
                        Session("SelectCommand") = "EXEC S_MsRptEmployeeCV " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", 0 "
                        Session("SelectCommand2") = "EXEC S_MsRptEmployeeCV " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", 1 "
                        Session("SelectCommand3") = "EXEC S_MsRptEmployeeCV " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", 2 "
                        Session("SelectCommand4") = "EXEC S_MsRptEmployeeCV " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", 3 "
                        Session("SelectCommand5") = "EXEC S_MsRptEmployeeCV " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", 4 "
                        Session("SelectCommand6") = "EXEC S_MsRptEmployeeCV " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", 5 "
                        Session("SelectCommand7") = "EXEC S_MsRptEmployeeCV " + QuotedStr(GVR.Cells(2).Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", 6 "
                        Session("ReportFile") = ".../../../Rpt/FormEmployeeCV.frx"
                        Session("DBConnection") = ViewState("DBConnection")
                        AttachScript("openprintdlg7ds();", Page, Me.GetType)
                    Catch ex As Exception
                        lbStatus.Text = "btn print Error = " + ex.ToString
                    End Try
                End If
            End If


        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Emp_No = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToText(tbName, Dt.Rows(0)("Emp_Name").ToString)
            BindToText(tbCandidate, Dt.Rows(0)("ApplicantNo").ToString)
            BindToDropList(ddlGender, Dt.Rows(0)("Gender").ToString)
            BindToDropList(ddlBloodType, Dt.Rows(0)("BloodType").ToString)
            BindToText(tbTribe, Dt.Rows(0)("Tribe").ToString)
            BindToDropList(ddlTypeCard, Dt.Rows(0)("TypeCard").ToString)
            BindToText(tbIDCard, Dt.Rows(0)("IDCard").ToString)
            BindToDropList(ddlResAddrStatus, Dt.Rows(0)("Res_AddrStatus").ToString)
            BindToDropList(ddlStatus, Dt.Rows(0)("TKAStatus").ToString)
            BindToText(tbLastNo, Dt.Rows(0)("LastCertificateNo").ToString)
            BindToDate(tbStartDate, Dt.Rows(0)("Hire_Date").ToString)
            BindToDate(tbBirthDate, Dt.Rows(0)("Birth_Date").ToString)
            BindToText(tbBirthPlace, Dt.Rows(0)("Birth_Place").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("End_Date").ToString)
            BindToDropList(ddlReligion, Dt.Rows(0)("Religion").ToString)
            BindToText(tbWeight, Dt.Rows(0)("Weight").ToString)
            BindToText(tbHeight, Dt.Rows(0)("Height").ToString)
            BindToText(tbPhoneContact, Dt.Rows(0)("HandPhone").ToString)
            BindToText(tbEmail, Dt.Rows(0)("Email").ToString)
            BindToDropList(ddlMaritalSt, Dt.Rows(0)("MaritalSt").ToString)
            BindToDropList(ddlMaritalTax, Dt.Rows(0)("MaritalTax").ToString)
            BindToDropList(ddlActive, Dt.Rows(0)("Fg_Active").ToString)
            BindToDropList(ddlSalaryType, Dt.Rows(0)("Salary_Type").ToString)
            BindToText(tbNPWP, Dt.Rows(0)("NPWP").ToString)
            BindToText(tbCard, Dt.Rows(0)("AbsenceCard").ToString)
            BindToDropList(ddlJobTitle, Dt.Rows(0)("Job_Title_Code").ToString)
            BindToDropList(ddlJobLevel, Dt.Rows(0)("Job_Level_Code").ToString)
            BindToDropList(ddlEmpStatus, Dt.Rows(0)("Emp_Status_Code").ToString)
            BindToDropList(ddlWorkPlace, Dt.Rows(0)("Work_Place_Code").ToString)
            BindToDropList(ddlMethod, Dt.Rows(0)("MethodSalary").ToString)
            'BindToDropList(ddlSubSection, Dt.Rows(0)("Sub_Section").ToString)
            BindToDropList(ddlDepartment, Dt.Rows(0)("Dept_Code").ToString)
            BindToDropList(ddlTKA, Dt.Rows(0)("FgTKA").ToString)
            BindToDropList(ddlFgJamsosTek, Dt.Rows(0)("FgJamsosTek").ToString)
            BindToDropList(ddlResCity, Dt.Rows(0)("Res_City").ToString)
            BindToDropList(ddlOriCity, Dt.Rows(0)("Ori_City").ToString)
            BindToText(tbSKNo, Dt.Rows(0)("SKNo").ToString)
            BindToText(tbAKDHK, Dt.Rows(0)("AKDHKNo").ToString)
            BindToText(tbPinBB, Dt.Rows(0)("PINBB").ToString)
            BindToText(tbjamsostek, Dt.Rows(0)("JamSosTekNo").ToString)
            BindToText(tbResAddr1, Dt.Rows(0)("Res_Addr1").ToString)
            BindToText(tbResAddr2, Dt.Rows(0)("Res_Addr2").ToString)
            BindToText(tbResZipCode, Dt.Rows(0)("Res_PostCode").ToString)
            BindToText(tbOriAddr1, Dt.Rows(0)("Ori_Addr1").ToString)
            BindToText(tbOriAddr2, Dt.Rows(0)("Ori_Addr2").ToString)
            BindToText(tbOriZipCode, Dt.Rows(0)("Ori_PostCode").ToString)
            BindToText(tbResPhone, Dt.Rows(0)("Res_Phone").ToString)
            BindToText(tbOriPhone, Dt.Rows(0)("Ori_Phone").ToString)
            BindToText(tbMaritalDocNo, Dt.Rows(0)("MaritalDocNo").ToString)
            BindToDate(tbJamsostekDate, Dt.Rows(0)("JamSosTekDate").ToString)
            BindToDate(tbDateMarital, Dt.Rows(0)("MaritalDate").ToString)
            If Dt.Rows(0)("End_Date_Contract").ToString <> "" Then
                BindToDate(tbEndContract, Dt.Rows(0)("End_Date_Contract").ToString)
            Else : tbEndContract.Clear()

            End If


        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
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
            'BindData(2, Session("StrResult"))
            BindData(1)
        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt.PageIndexChanging
        Try
            GridDt.PageIndex = e.NewPageIndex
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt.RowCommand
        Try
            If CheckMenuLevel("Insert") = False Then
                Exit Sub
            End If
            If e.CommandName = "Insert" Then
            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt.RowDeleting
        'Dim dr() As DataRow
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)

        SQLExecuteNonQuery("Delete from MsEmpLanguage where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + GVR.Cells(1).Text, ViewState("DBConnection").ToString)
        BindDataDt(tbCode.Text)
        GridDt.DataSource = ViewState("Dt")

        lbStatus.Text = MessageDlg("Data Deleted")
        EnableHd(GridDt.Rows.Count = 0)
    End Sub

    Protected Sub GridDt_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridDt.Rows(e.NewEditIndex)

            lbItemNo.Text = GVR.Cells(1).Text
            BindToText(tbLanguage, GVR.Cells(2).Text)
            BindToDropList(ddlRead, GVR.Cells(3).Text)
            BindToDropList(ddlWrite, GVR.Cells(4).Text)
            BindToDropList(ddlSpeak, GVR.Cells(5).Text)
            BindToText(tbexam, GVR.Cells(6).Text)
            BindToText(tbGPD, GVR.Cells(7).Text)
            pnlEditDt.Visible = True
            pnlDt.Visible = False
            ViewState("StateDt") = "Edit"
            tbLanguage.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveHd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveHd.Click
        Try
            SaveHd()
        Catch ex As Exception
            lbStatus.Text = "Save Hd Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelHd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelHd.Click
        Try
            pnlView.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReset.Click
        Try
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If
            ClearHd()
        Catch ex As Exception
            lbStatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnadddt.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        cleardt()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("Dt").select("")
            i = Row.Length
        End If

        ViewState("StateDt") = "Insert"
        If i > 0 Then
            lbItemNo.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNo.Text = "1"
        End If
        pnlEditDt.Visible = True
        pnlDt.Visible = False
        EnableHd(False)
        tbLanguage.Focus()
        Exit Sub
    End Sub

    Private Sub SaveDt1()
        Dim SQLString As String
        Try
            If tbLanguage.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Language Must Have Value")
                tbLanguage.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsEmpLanguage WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNo.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Language " + QuotedStr(lbItemNo.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpLanguage (EmpNumb, ItemNo, LanguageName, GradeRead, GradeWrite, GradeSpeak, Exam, GPD) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemNo.Text + ", " + QuotedStr(tbLanguage.Text) + ", " + QuotedStr(ddlRead.SelectedValue) + ", " + QuotedStr(ddlWrite.SelectedValue) + ", " + QuotedStr(ddlSpeak.SelectedValue) + ", " + QuotedStr(tbexam.Text) + ", " + _
                QuotedStr(tbGPD.Text)

            Else
                SQLString = "UPDATE MsEmpLanguage SET GradeRead = " + QuotedStr(ddlRead.SelectedValue) + ", GradeWrite = " + QuotedStr(ddlWrite.SelectedValue) + ", " + _
                " GradeSpeak = " + QuotedStr(ddlSpeak.SelectedValue) + ", Exam = " + QuotedStr(tbexam.Text) + ", " + _
                " GPD = " + QuotedStr(tbGPD.Text) + ", LanguageName = " + QuotedStr(tbLanguage.Text) + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNo.Text) + " "
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditDt.Visible = False
            pnlDt.Visible = True
            EnableHd(GridDt.Rows.Count = 0)
            BindDataDt(ViewState("EmpNumb"))
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Dt1 Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelExp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelExp.Click
        Try
            PnlEditExp.Visible = False
            PnlExp.Visible = True
            'EnableHd(GridDt.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Addr Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridExp_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridExp.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridExp.Rows(e.NewEditIndex)
            lbItemNoExp.Text = GVR.Cells(1).Text
            BindToText(tbcompanyName, GVR.Cells(2).Text)
            BindToDropList(ddlCompanyBusiness, GVR.Cells(3).Text)
            BindToText(tbAddress1Addr, GVR.Cells(4).Text)
            BindToText(tbAddress2Addr, GVR.Cells(5).Text)
            BindToDropList(ddlCityExp, GVR.Cells(6).Text)
            BindToText(tbPhoneExp, GVR.Cells(7).Text)
            BindToDropList(ddlJobTitleExp, GVR.Cells(8).Text)
            BindToDropList(ddlDeptExp, GVR.Cells(9).Text)
            BindToText(tbsalaryExp, GVR.Cells(10).Text)
            BindToText(tbPHKReason, GVR.Cells(11).Text)
            BindToText(tbWorkPeriod, GVR.Cells(12).Text)
            PnlEditExp.Visible = True
            PnlExp.Visible = False
            EnableHd(False)
            ViewState("StateDtExp") = "Edit"
            tbcompanyName.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid View Addr Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridExp_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridExp.RowDeleting
        Dim SQLString As String
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridExp.Rows(e.RowIndex)

        SQLString = "Delete from MsEmpExp where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(GVR.Cells(1).Text)
        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        GridExp.DataSource = ViewState("DtExp")
        BindDataDtExp(tbCode.Text)
        lbStatus.Text = MessageDlg("Data Deleted")
        'EnableHd(GridExp.Rows.Count = 0)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsearch.Click
        Try
            BindData(1)
            pnlNav.Visible = False
            pnlDt.Visible = False
            PnlExp.Visible = False
            PnlSkill.Visible = False
            PnlP.Visible = False
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddP.Click
        cleardtP()
        ' newTrans()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtP").select("")
            i = Row.Length
        End If
        ViewState("StateDtP") = "Insert"
        PnlEditPrestasi.Visible = True
        PnlP.Visible = False
        EnableHd(False)
        tbtahunP.Enabled = True
        tbtahunP.Focus()
        Exit Sub
    End Sub

    Private Sub SaveDtP()
        Dim SQLString As String
        Try
            If tbtahunP.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Tahun Must Have Value")
                tbtahunP.Focus()
                Exit Sub
            End If
            If tbprestasi.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Prestasi Must Have Value")
                tbprestasi.Focus()
                Exit Sub
            End If

            If ViewState("StateDtP") = "Insert" Then
                If SQLExecuteScalar("SELECT Tahun, Prestasi FROM MsEmpPrestasi WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND Tahun = " + tbtahunP.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "Detail Prestasi Info " + QuotedStr(tbtahunP.Text) + " has already been exist"
                    Exit Sub
                End If


                SQLString = "INSERT INTO MsEmpPrestasi (EmpNumb, Tahun, Prestasi) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbtahunP.Text) + ", " + _
                QuotedStr(tbprestasi.Text)
            Else
                SQLString = "UPDATE MsEmpPrestasi SET Tahun = " + QuotedStr(tbtahunP.Text) + _
                ", Prestasi = " + QuotedStr(tbprestasi.Text) + _
                " WHERE EmpNumb = '" + tbCode.Text + "' AND Tahun = '" + tbtahunP.Text + "' "

            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditPrestasi.Visible = False
            PnlP.Visible = True
            EnableHd(GridP.Rows.Count = 0)
            BindDataPrestasi(ViewState("EmpNumb"))
            GridP.DataSource = ViewState("DtP")
            GridP.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Dt P Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataPrestasi(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtP") = Nothing
            dt = SQLExecuteQuery(GetStringDtPrestasi(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtP") = dt
            GridP.DataSource = dt
            GridP.DataBind()
            BindGridDt(dt, GridP)
        Catch ex As Exception
            Throw New Exception("Bind Data P Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSavep.Click
        Try
            SaveDtP()
            PnlP.Visible = True
            PnlEditPrestasi.Visible = False
            GridP.PageIndex = 0
            BindDataPrestasi(ViewState("EmpNumb"))
        Catch ex As Exception
            lbStatus.Text = "Save P Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelp.Click
        Try
            PnlP.Visible = True
            PnlEditPrestasi.Visible = False
            EnableHd(GridP.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel P Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridP_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridP.PageIndexChanging
        Try
            GridP.PageIndex = e.NewPageIndex
            GridP.DataSource = ViewState("DtP")
            GridP.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid View Filed Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridP_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridP.RowCommand
        Try
            If e.CommandName = "Insert" Then

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid View P Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridP_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridP.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridP.Rows(e.NewEditIndex)
            tbtahunP.Text = GVR.Cells(1).Text
            tbprestasi.Text = GVR.Cells(2).Text
            PnlEditPrestasi.Visible = True
            PnlP.Visible = False
            EnableHd(False)
            tbtahunP.Enabled = False
            ViewState("StateDtP") = "Edit"
            tbprestasi.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid View P Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveDtExp()
        Dim SQLString As String
        Try
            If tbcompanyName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Company Name Must Have Value")
                tbcompanyName.Focus()
                Exit Sub
            End If
            If ViewState("StateDtExp") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsEmpExp WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNoExp.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Experience " + QuotedStr(lbItemNoExp.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpExp(EmpNumb, ItemNo, CompanyName, CompanyBusiness, WorkPeriod, Address1, Address2, CompanyCity, Phone,  JobTitle,  Department, JobResponsibility, Currency, LastSalary, PHKReason) " + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemNoExp.Text + ", " + QuotedStr(tbcompanyName.Text) + ", " + QuotedStr(ddlCompanyBusiness.SelectedValue) + ", " + QuotedStr(tbWorkPeriod.Text) + ", " + QuotedStr(tbAddress1Addr.Text) + ", " + QuotedStr(tbAddress2Addr.Text) + ", " + _
               QuotedStr(ddlCityExp.SelectedValue) + ", " + QuotedStr(tbPhoneExp.Text) + ", " + QuotedStr(ddlJobTitleExp.SelectedValue) + ", " + QuotedStr(ddlDeptExp.SelectedValue) + ", " + QuotedStr(tbJobDesc.Text) + ", " + QuotedStr(ViewState("Currency").ToString) + ", " + tbsalaryExp.Text.Replace(",", "") + ", " + _
               QuotedStr(tbPHKReason.Text)
            Else
                SQLString = "UPDATE MsEmpExp SET EmpNumb = " + QuotedStr(tbCode.Text) + ", " + _
                " CompanyName = " + QuotedStr(tbcompanyName.Text) + ", CompanyBusiness = " + QuotedStr(ddlCompanyBusiness.SelectedValue) + ", " + _
                " WorkPeriod = " + QuotedStr(tbWorkPeriod.Text) + ", Address1 = " + QuotedStr(tbAddress1Addr.Text) + ", " + _
                " Address2 = " + QuotedStr(tbAddress2Addr.Text) + ", CompanyCity = " + QuotedStr(ddlCityExp.SelectedValue) + ", " + _
                " Phone = " + QuotedStr(tbPhoneExp.Text) + ", JobTitle = " + QuotedStr(ddlJobTitleExp.SelectedValue) + ", " + _
                " Department = " + QuotedStr(ddlDeptExp.SelectedValue) + ", JobResponsibility = " + QuotedStr(tbJobDesc.Text) + ", " + _
                " Currency = " + QuotedStr(ViewState("Currency").ToString) + ", LastSalary = " + tbsalaryExp.Text.Replace(",", "") + ", " + _
                " PHKReason = " + QuotedStr(tbPHKReason.Text) + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNoExp.Text
            End If
            
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditExp.Visible = False
            PnlExp.Visible = True
            EnableHd(GridDt.Rows.Count = 0)
            BindDataDtExp(ViewState("EmpNumb"))
            GridExp.DataSource = ViewState("DtExp")
            GridExp.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Dt Addr Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub SaveDtT()
        Dim SQLString As String
        Try
            If tbTrainingCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Trainning Code Must Have Value")
                tbTrainingCode.Focus()
                Exit Sub
            End If
            If tbTrainingPlace.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Trainning Place Must Have Value")
                tbTrainingPlace.Focus()
                Exit Sub
            End If
            If ddlCostType.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Cost Type Must Have Value")
                ddlCostType.Focus()
                Exit Sub
            End If
            If ViewState("StateDtT") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo, TrainingCode, TrainingName, TrainingPlace, TrainingPeriod, Institution, Location, TutorName, Nilai, CostType, " + _
                " FgCertificate, Certificate, UserId, UserDate FROM MsEmpTraining WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNoT.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Training " + QuotedStr(tbCode.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpTraining (EmpNumb, ItemNo, TrainingCode, TrainingName, TrainingPlace, TrainingPeriod, Institution, Location, TutorName, Nilai, CostType, " + _
                " FgCertificate, Certificate, UserId, UserDate )" + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbItemNoT.Text) + ", " + QuotedStr(tbTrainingCode.Text) + ", " + QuotedStr(tbTrainingName.Text) + ", " + _
               QuotedStr(tbTrainingPlace.Text) + ", " + QuotedStr(tbTrainingPeriod.Text) + ", " + QuotedStr(tbInstitution.Text) + ", " + _
               QuotedStr(tbLocation.Text) + " , " + QuotedStr(tbTutorName.Text) + ", " + QuotedStr(tbNilai.Text) + ", " + QuotedStr(ddlCostType.SelectedValue) + ", " + _
               QuotedStr(ddlFgCertificate.SelectedValue) + ", " + QuotedStr(tbCertificateT.Text) + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                SQLString = "UPDATE MsEmpTraining SET TrainingCode = " + QuotedStr(tbTrainingCode.Text) + ", TrainingName = " + QuotedStr(tbTrainingName.Text) + ", " + _
                " TrainingPlace = " + QuotedStr(tbTrainingPlace.Text) + ", TrainingPeriod= " + QuotedStr(tbTrainingPeriod.Text) + ", " + _
                " Institution = " + QuotedStr(tbInstitution.Text) + ", Location = " + QuotedStr(tbLocation.Text) + ", CostType = " + QuotedStr(ddlCostType.SelectedValue) + ", " + _
                " TutorName = " + QuotedStr(tbTutorName.Text) + ", Nilai = " + QuotedStr(tbNilai.Text) + ", Certificate = " + QuotedStr(tbCertificateT.Text) + ", " + _
                " FgCertificate = " + QuotedStr(ddlFgCertificate.SelectedValue) + ", UserDate = getDate()" + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNoT.Text) + " "
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            pnlEditTraining.Visible = False
            pnlTraining.Visible = True
            Gridt.PageIndex = 0
            BindDataDtTraining(ViewState("EmpNumb"))
        Catch ex As Exception
            Throw New Exception("Save Dt Addr Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDtTraining(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtT") = Nothing
            dt = SQLExecuteQuery(GetStringDtTraining(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtT") = dt
            Gridt.DataSource = dt
            Gridt.DataBind()
            BindGridDt(dt, Gridt)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Addr Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveT.Click
        Try
            SaveDtT()
        Catch ex As Exception
            lbStatus.Text = "Save Addr Error : " + ex.ToString
        End Try
    End Sub

    Private Sub cleardtExp()
        Try
            lbItemNoExp.Text = ""
            tbcompanyName.Text = ""
            ddlCompanyBusiness.SelectedIndex = 0
            tbAddress1Addr.Text = ""
            tbAddress2Addr.Text = ""
            ddlCityExp.SelectedValue = ""
            tbPhoneExp.Text = ""
            ddlJobTitleExp.SelectedValue = ""
            ddlDeptExp.SelectedValue = ""
            tbJobDesc.Text = ""
            tbsalaryExp.Text = "0"
            tbPHKReason.Text = ""
            tbWorkPeriod.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Bill To Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveExp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveExp.Click
        Try
            SaveDtExp()
        Catch ex As Exception
            Throw New Exception("Save Dt Exp Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridExp_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridExp.PageIndexChanging
        Try
            GridExp.PageIndex = e.NewPageIndex
            GridExp.DataSource = ViewState("DtExp")
            GridExp.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid View Bill To Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Private Sub BindDataDtExp(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtExp") = Nothing
            dt = SQLExecuteQuery(GetStringDtExp(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtExp") = dt
            GridExp.DataSource = dt
            GridExp.DataBind()
            BindGridDt(dt, GridExp)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Addr Error : " + ex.ToString)
        End Try
    End Sub

    'Protected Sub btnExp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExp.Click
    '    Dim FieldResult As String
    '    Try
    '        Session("filter") = "SELECT Employee_Code, Employee_Name FROM vMsEmployee"
    '        FieldResult = "Employee_Code, Employee_Name"
    '        Session("Column") = FieldResult.Split(",")
    '        Session("Sender") = "btnExp"
    '        If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
    '            Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
    '        End If
    '    Catch ex As Exception
    '        lbStatus.Text = "btn Bill To Error : " + ex.ToString
    '    End Try
    'End Sub

    'Protected Sub tbExp_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbExp.TextChanged
    '    Dim DsExp As DataSet
    '    Dim DrExp As DataRow
    '    Try
    '        DsExp = SQLExecuteQuery("Select * from VMsEmployee WHERE Employee_Code = '" + tbExp.Text + "'")
    '        If DsExp.Tables(0).Rows.Count = 1 Then
    '            DrExp = DsExp.Tables(0).Rows(0)
    '            tbExp.Text = DrExp("Employee_Code")
    '            tbExpName.Text = DrExp("Employee_Name")
    '        Else
    '            tbExp.Text = ""
    '            tbExpName.Text = ""
    '        End If
    '        tbExp.Focus()
    '    Catch ex As Exception
    '        Throw New Exception("tb Bill To Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnAddExp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddExp.Click
        cleardtExp()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtExp").select("")
            i = Row.Length
        End If

        ViewState("StateDtExp") = "Insert"
        If i > 0 Then
            lbItemNoExp.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNoExp.Text = "1"
        End If

        PnlEditExp.Visible = True
        PnlExp.Visible = False
        EnableHd(False)
        '  tbDeliveryCode.Focus()
        Exit Sub
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            SaveDt1()
        Catch ex As Exception
            lbStatus.Text = "Save Contact Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub GridP_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridP.RowDeleting
        'Dim dr() As DataRow
        Dim SQlString As String
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridP.Rows(e.RowIndex)
        'dr = ViewState("DtP").Select("Tahun = " + GVR.Cells(1).Text)
        'dr(0).Delete()

        SQlString = "Delete from MsEmpPrestasi where EmpNumb = " + QuotedStr(tbCode.Text) + " AND Tahun = " + QuotedStr(GVR.Cells(1).Text)
        SQLExecuteNonQuery(SQlString, ViewState("DBConnection").ToString)
        BindDataPrestasi(tbCode.Text)
        'ViewState("DtP").AcceptChanges()
        'GridP.DataSource = ViewState("DtP")
        'GridP.DataBind()
        lbStatus.Text = MessageDlg("Data Deleted")
        EnableHd(GridP.Rows.Count = 0)
    End Sub

    Protected Sub GridT_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gridt.PageIndexChanging
        Try
            Gridt.PageIndex = e.NewPageIndex
            Gridt.DataSource = ViewState("DtT")
            Gridt.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid View T Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView2.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
            ElseIf Menu1.Items(10).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                Catch ex As Exception
                    lbStatus.Text = "DDL.SelectedValue = 4 Error : " + ex.ToString
                End Try
            End If
        Next
    End Sub

    Protected Sub btnprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnprint.Click
        Dim StrFilter As String
        Try
            If tbFilter.Text.Trim.Length > 0 Then
                If tbfilter2.Text.Trim.Length > 0 And pnlSearch.Visible Then
                    If ddlField.SelectedValue = "Gender" Then
                        StrFilter = " WHERE " + ddlField.SelectedValue + " = ''" + tbFilter.Text + "'' " + _
                        ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " Like ''%" + tbfilter2.Text + "%''"
                    ElseIf ddlField2.SelectedValue = "Gender" Then
                        StrFilter = " WHERE " + ddlField.SelectedValue + " Like ''%" + tbFilter.Text + "%'' " + _
                        ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " = ''" + tbfilter2.Text + "''"
                    ElseIf ddlField.SelectedValue = "Gender" And ddlField2.SelectedValue = "Gender" Then
                        StrFilter = " WHERE " + ddlField.SelectedValue + " = ''" + tbFilter.Text + "'' " + _
                        ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " = ''" + tbfilter2.Text + "''"
                    Else
                        StrFilter = " WHERE " + ddlField.SelectedValue + " like ''%" + tbFilter.Text + "%'' " + _
                        ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " like ''%" + tbfilter2.Text + "%''"
                    End If
                Else
                    If ddlField.SelectedValue = "Gender" Then
                        StrFilter = " WHERE A.EmpNumb IS NOT NULL AND " + ddlField.SelectedValue + " = ''" + tbFilter.Text + "''"
                    Else
                        StrFilter = " WHERE A.EmpNumb IS NOT NULL AND " + ddlField.SelectedValue + " like ''%" + tbFilter.Text + "%''"
                    End If

                End If
            Else
                StrFilter = " Where A.EmpNumb IS NOT NULL "
            End If
            'lbStatus.Text = StrFilter
            'Exit Sub
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_MsRptEmployee '" + StrFilter + "'"
            Session("ReportFile") = Server.MapPath("~\Rpt\RptEmployee.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu2_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu2.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        For i = 0 To Menu2.Items.Count - 1
            If i = e.Item.Value Then
            ElseIf Menu2.Items(3).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                Catch ex As Exception
                    lbStatus.Text = "DDL.SelectedValue = 4 Error : " + ex.ToString
                End Try

            End If
        Next
    End Sub
    Private Sub BindDataDtFamily(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtF") = Nothing
            dt = SQLExecuteQuery(GetStringDtFamily(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtF") = dt
            Gridf.DataSource = dt
            Gridf.DataBind()
            BindGridDt(dt, Gridf)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Gridf_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gridf.PageIndexChanging
        Try
            Gridf.PageIndex = e.NewPageIndex
            Gridf.DataSource = ViewState("DtF")
            Gridf.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub Gridf_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Gridf.RowDeleting
        'Dim dr() As DataRow
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = Gridf.Rows(e.RowIndex)

        SQLExecuteNonQuery("Delete from MsEmpFamily where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + GVR.Cells(1).Text, ViewState("DBConnection").ToString)
        BindDataDtFamily(tbCode.Text)
        Gridf.DataSource = ViewState("DtF")
        lbStatus.Text = MessageDlg("Data Deleted ")
        EnableHd(Gridf.Rows.Count = 0)
    End Sub
    Private Sub cleardtf()
        Try
            lbItemNoF.Text = ""
            ddlfamilyType.SelectedIndex = 0
            tbFamilyName.Text = ""
            tbOccupation.Text = ""
            tbAddress1.Text = ""
            tbAddress2.Text = ""
            ddlFgMedical.SelectedIndex = 0
            tbPhone.Text = ""
            tbBirthPlaceF.Text = ""
            tbBirthDateFamily.Clear()
            ddlGenderFamily.SelectedIndex = 0
            ddlEduLevel.SelectedValue = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub


    Protected Sub btnaddf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddf.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        cleardtf()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtF").select("")
            i = Row.Length
        End If

        ViewState("StateDtF") = "Insert"
        If i > 0 Then
            lbItemNoF.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNoF.Text = "1"
        End If
        PnlEditFamily.Visible = True
        Pnlfamily.Visible = False
        EnableHd(False)
        ddlfamilyType.Focus()
        Exit Sub
    End Sub

    Protected Sub Gridf_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Gridf.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = Gridf.Rows(e.NewEditIndex)

            lbItemNoF.Text = GVR.Cells(1).Text
            ddlfamilyType.SelectedValue = GVR.Cells(2).Text.Replace("&nbsp;", "")
            tbFamilyName.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbOccupation.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbAddress1.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbAddress2.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            tbPhone.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            tbBirthPlaceF.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            BindToDate(tbBirthDateFamily, GVR.Cells(9).Text)
            ddlGenderFamily.SelectedValue = GVR.Cells(10).Text.Replace("&nbsp;", "")
            ddlEduLevel.SelectedValue = GVR.Cells(11).Text.Replace("&nbsp;", "")
            ddlFgMedical.SelectedValue = GVR.Cells(12).Text.Replace("&nbsp;", "")
            PnlEditFamily.Visible = True
            Pnlfamily.Visible = False
            ViewState("StateDtF") = "Edit"
            tbFamilyName.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SaveDtF()
        Dim SQLString As String
        Try
            If ddlEduLevel.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Last Education Must Have Value")
                ddlEduLevel.Focus()
                Exit Sub
            End If

            If ViewState("StateDtF") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsEmpFamily WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNoF.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Family " + QuotedStr(lbItemNoF.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpFamily (EmpNumb, ItemNo, FamilyName, FamilyType, Occupation, Address1, Address2, FgMedical, Phone, BirthPlace, BirthDate, Gender, EduLevel) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemNoF.Text + ", " + QuotedStr(tbFamilyName.Text) + ", " + QuotedStr(ddlfamilyType.SelectedValue) + ", " + QuotedStr(tbOccupation.Text) + ", " + QuotedStr(tbAddress1.Text) + ", " + QuotedStr(tbAddress2.Text) + ", " + _
                QuotedStr(ddlFgMedical.SelectedValue) + ", " + QuotedStr(tbPhone.Text) + ", " + QuotedStr(tbBirthPlaceF.Text) + ", " + QuotedStr(Format(tbBirthDateFamily.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(ddlGenderFamily.SelectedValue) + ", " + QuotedStr(ddlEduLevel.SelectedValue.Replace("&nbsp;", ""))

            Else
                SQLString = "UPDATE MsEmpFamily SET FamilyName = " + QuotedStr(tbFamilyName.Text) + ", FamilyType = " + QuotedStr(ddlfamilyType.SelectedValue) + ", " + _
                " Occupation = " + QuotedStr(tbOccupation.Text) + ", Address1 = " + QuotedStr(tbAddress1.Text) + ", " + _
                " Address2 = " + QuotedStr(tbAddress2.Text) + ", FgMedical = " + QuotedStr(ddlFgMedical.SelectedValue) + ", " + _
                " Phone = " + QuotedStr(tbPhone.Text) + ", BirthPlace = " + QuotedStr(tbBirthPlaceF.Text) + ", " + _
                " BirthDate = " + QuotedStr(Format(tbBirthDateFamily.SelectedValue, "yyyy-MM-dd")) + ", EduLevel = " + QuotedStr(ddlEduLevel.SelectedValue) + ", " + _
                " Gender = " + QuotedStr(ddlGenderFamily.SelectedValue) + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNoF.Text) + " "
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditFamily.Visible = False
            Pnlfamily.Visible = True
            EnableHd(Gridf.Rows.Count = 0)
            BindDataDtFamily(ViewState("EmpNumb"))
            Gridf.DataSource = ViewState("DtF")
            Gridf.DataBind()

            PnlEditFamily.Visible = False
            Pnlfamily.Visible = True
            Gridf.PageIndex = 0
            BindDataDtFamily(ViewState("EmpNumb"))
        Catch ex As Exception
            Throw New Exception("Save Dt1 Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSavef_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsavef.Click
        Try
            SaveDtF()
            
        Catch ex As Exception
            lbStatus.Text = "Save Family Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelf_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancelf.Click
        Try
            PnlEditFamily.Visible = False
            Pnlfamily.Visible = True
            'EnableHd(GridDt.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Addr Error : " + ex.ToString
        End Try
    End Sub

    Private Sub BindDataDtSkill(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtS") = Nothing
            dt = SQLExecuteQuery(GetStringDtSkill(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtS") = dt
            GridS.DataSource = dt
            GridS.DataBind()
            BindGridDt(dt, GridS)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub GridS_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridS.PageIndexChanging
        Try
            GridS.PageIndex = e.NewPageIndex
            GridS.DataSource = ViewState("DtS")
            GridS.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub
    Protected Sub GridS_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridS.RowDeleting
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridS.Rows(e.RowIndex)
        SQLExecuteNonQuery("Delete from MsEmpSkill where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + GVR.Cells(1).Text, ViewState("DBConnection").ToString)
        GridS.DataSource = ViewState("DtS")
        BindDataDtSkill(tbCode.Text)
        lbStatus.Text = MessageDlg("Data Deleted")
        EnableHd(GridS.Rows.Count = 0)
    End Sub
    Private Sub cleardtS()
        Try
            lbItemNoSkill.Text = ""
            ddlSkillType.SelectedIndex = 0
            tbSkilName.Text = ""
            ddlSkillGrade.SelectedIndex = 0
            tbInternalAction.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Skill Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnaddS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdds.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        cleardtS()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtS").select("")
            i = Row.Length
        End If

        ViewState("StateDtS") = "Insert"
        If i > 0 Then
            lbItemNoSkill.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNoSkill.Text = "1"
        End If
        PnlEditSkill.Visible = True
        PnlSkill.Visible = False
        EnableHd(False)
        tbSkilName.Focus()
        Exit Sub
    End Sub
    Protected Sub GridS_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridS.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridS.Rows(e.NewEditIndex)

            lbItemNoSkill.Text = GVR.Cells(1).Text
            BindToDropList(ddlSkillType, GVR.Cells(2).Text)
            BindToText(tbSkilName, GVR.Cells(3).Text)
            BindToDropList(ddlSkillGrade, GVR.Cells(4).Text)
            BindToText(tbInternalAction, GVR.Cells(5).Text)
            PnlEditSkill.Visible = True
            PnlSkill.Visible = False
            ViewState("StateDtS") = "Edit"
            ddlSkillType.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SaveDtS()
        Dim SQLString As String
        Try
            
            If ViewState("StateDtS") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsEmpSkill WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNoSkill.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Skill " + QuotedStr(lbItemNoSkill.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpSkill (EmpNumb, ItemNo, SkillType, SkillName, SkillGrade, InternalAction) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemNoSkill.Text + ", " + QuotedStr(ddlSkillType.SelectedValue) + ", " + QuotedStr(tbSkilName.Text) + ", " + QuotedStr(ddlSkillGrade.SelectedValue) + ", " + QuotedStr(tbInternalAction.Text)

            Else
                SQLString = "UPDATE MsEmpSkill SET SkillType = " + QuotedStr(ddlSkillType.SelectedValue) + ", SkillName = " + QuotedStr(tbSkilName.Text) + ", " + _
                " SkillGrade = " + QuotedStr(ddlSkillGrade.SelectedValue) + ", InternalAction = " + QuotedStr(tbInternalAction.Text) + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNoSkill.Text)

            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditSkill.Visible = False
            PnlSkill.Visible = True
            EnableHd(GridS.Rows.Count = 0)
            BindDataDtSkill(ViewState("EmpNumb"))
            GridS.DataSource = ViewState("DtS")
            GridS.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Skill Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaves.Click
        Try
            SaveDtS()
            PnlEditSkill.Visible = False
            PnlSkill.Visible = True
            GridS.PageIndex = 0
            BindDataDtSkill(ViewState("EmpNumb"))
        Catch ex As Exception
            lbStatus.Text = "Save Skill Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancels.Click
        Try
            PnlEditSkill.Visible = False
            PnlSkill.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub


    Private Sub BindDataDtEdu(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtEdu") = Nothing
            dt = SQLExecuteQuery(GetStringDtEdu(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtEdu") = dt
            GridEdu.DataSource = dt
            GridEdu.DataBind()
            BindGridDt(dt, GridEdu)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Edu Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub GridEdu_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridEdu.PageIndexChanging
        Try
            GridEdu.PageIndex = e.NewPageIndex
            GridEdu.DataSource = ViewState("DtEdu")
            GridEdu.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub
    Protected Sub GridEdu_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridEdu.RowDeleting
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridEdu.Rows(e.RowIndex)

        SQLExecuteNonQuery("Delete from MsEmpEdu where EmpNumb = " + QuotedStr(tbCode.Text) + " AND EduLevel = " + GVR.Cells(1).Text, ViewState("DBConnection").ToString)
        GridEdu.DataSource = ViewState("DtEdu")
        BindDataDtEdu(tbCode.Text)
        lbStatus.Text = MessageDlg("Data Deleted")
        EnableHd(GridEdu.Rows.Count = 0)
    End Sub
    Private Sub cleardtEdu()
        Try
            ddlEduLevelEdu.SelectedValue = ""
            tbSchollName.Text = ""
            tbAddr1Edu.Text = ""
            tbAddr2Edu.Text = ""
            ddlCityEdu.SelectedValue = ""
            tbMajor.Text = ""
            tbCertificate.Text = ""
            tbGPA.Text = "0"
            tbPeriod.Text = ""
            ddlGraduated.SelectedIndex = 0
            tbGraduated.Text = "0"
            ViewState("EduLevel") = Nothing
        Catch ex As Exception
            Throw New Exception("Clear Skill Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnaddEdu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddEdu.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        cleardtEdu()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtEdu").select("")
            i = Row.Length
        End If

        ViewState("StateDtEdu") = "Insert"
        pnlEditEdu.Visible = True
        PnlEdu.Visible = False
        EnableHd(False)
        ddlGraduated_SelectedIndexChanged(Nothing, Nothing)
        ddlEduLevelEdu.Enabled = True
        ddlEduLevelEdu.Focus()
        Exit Sub
    End Sub
    Protected Sub GridEdu_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridEdu.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridEdu.Rows(e.NewEditIndex)

            ddlEduLevelEdu.SelectedValue = GVR.Cells(1).Text
            BindToText(tbSchollName, GVR.Cells(2).Text)
            BindToText(tbAddr1Edu, GVR.Cells(3).Text)
            BindToText(tbAddr2Edu, GVR.Cells(4).Text)
            BindToDropList(ddlCityEdu, GVR.Cells(5).Text)
            BindToText(tbMajor, GVR.Cells(6).Text)
            BindToText(tbCertificate, GVR.Cells(7).Text)
            BindToText(tbGPA, GVR.Cells(8).Text)
            BindToText(tbPeriod, GVR.Cells(9).Text)
            BindToDropList(ddlGraduated, GVR.Cells(10).Text)
            BindToText(tbGraduated, GVR.Cells(11).Text)
            pnlEditEdu.Visible = True
            PnlEdu.Visible = False
            ddlGraduated_SelectedIndexChanged(Nothing, Nothing)
            ViewState("StateDtEdu") = "Edit"
            ViewState("EduLevel") = GVR.Cells(1).Text
            ddlEduLevelEdu.Enabled = False
            tbSchollName.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SaveDtEdu()
        Dim SQLString As String
        Try
            If ddlEduLevelEdu.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Education Level Must Have Value")
                ddlEduLevelEdu.Focus()
                Exit Sub
            End If
            If tbSchollName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Scholl Name Must Have Value")
                tbSchollName.Focus()
                Exit Sub
            End If

            If ViewState("StateDtEdu") = "Insert" Then
                If SQLExecuteScalar("SELECT EduLevel FROM MsEmpEdu WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND EduLevel = " + QuotedStr(ddlEduLevelEdu.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Education " + QuotedStr(ddlEduLevelEdu.SelectedValue) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpEdu (EmpNumb, EduLevel, SchoolName, EduMajor, Address1, Address2, EduCity, DurasiYear, GPA, CertificateNo, FgGraduated, Graduated) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(ddlEduLevelEdu.SelectedValue).Replace("&nbsp;", "") + ", " + QuotedStr(tbSchollName.Text) + ", " + QuotedStr(tbMajor.Text) + ", " + QuotedStr(tbAddr1Edu.Text) + ", " + QuotedStr(tbAddr2Edu.Text) + ", " + _
                QuotedStr(ddlCityEdu.SelectedValue.Replace("&nbsp;", "")) + ", " + QuotedStr(tbPeriod.Text) + ", " + tbGPA.Text.Replace(",", "") + ", " + QuotedStr(tbCertificate.Text) + ", " + QuotedStr(ddlGraduated.SelectedValue) + ", " + tbGraduated.Text.Replace(",", "")

            Else
                SQLString = "UPDATE MsEmpEdu SET Address2 = " + QuotedStr(tbAddr2Edu.Text) + ", SchoolName = " + QuotedStr(tbSchollName.Text) + ", " + _
                " EduMajor = " + QuotedStr(tbMajor.Text) + ", Address1 = " + QuotedStr(tbAddr1Edu.Text) + ", " + _
                " EduCity = " + QuotedStr(ddlCityEdu.SelectedValue) + ", DurasiYear = " + QuotedStr(tbPeriod.Text) + ", " + _
                " GPA = " + tbGPA.Text.Replace(",", "") + ", CertificateNo = " + QuotedStr(tbCertificate.Text) + ", " + _
                " Graduated = " + tbGraduated.Text.Replace(", ", "") + ", FgGraduated = " + QuotedStr(ddlGraduated.SelectedValue) + ", " + _
                " EduLevel = " + QuotedStr(ddlEduLevelEdu.SelectedValue) + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND EduLevel = " + QuotedStr(ViewState("EduLevel"))
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditEdu.Visible = False
            PnlEdu.Visible = True
            EnableHd(GridEdu.Rows.Count = 0)
            BindDataDtEdu(ViewState("EmpNumb"))
            GridEdu.DataSource = ViewState("DtEdu")
            GridEdu.DataBind()
            'ddlEduLevelEdu.Enabled = True
            'pnlEditEdu.Visible = False
            'PnlEdu.Visible = True
            'GridEdu.PageIndex = 0
            'BindDataDtEdu(ViewState("EmpNumb"))
        Catch ex As Exception
            Throw New Exception("Save Skill Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveEdu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveEdu.Click
        Try
            SaveDtEdu()
        Catch ex As Exception
            lbStatus.Text = "Save Edu Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelEdu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancelEdu.Click
        Try
            pnlEditEdu.Visible = False
            PnlEdu.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Edu Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancelT.Click
        Try
            pnlEditTraining.Visible = False
            pnlTraining.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Edu Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnaddT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddT.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        clearT()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtT").select("")
            i = Row.Length
        End If

        ViewState("StateDtT") = "Insert"
        If i > 0 Then
            lbItemNoT.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNoT.Text = "1"
        End If
        pnlEditTraining.Visible = True
        pnlTraining.Visible = False
        EnableHd(False)
        tbTrainingCode.Focus()
        Exit Sub
    End Sub

    Protected Sub Gridt_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Gridt.RowDeleting
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = Gridt.Rows(e.RowIndex)
        SQLExecuteNonQuery("Delete from MsEmpTraining where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + GVR.Cells(1).Text, ViewState("DBConnection").ToString)
        Gridt.DataSource = ViewState("DtT")
        BindDataDtTraining(tbCode.Text)
        lbStatus.Text = MessageDlg("Data Deleted")
        'EnableHd(GridT.Rows.Count = 0)
    End Sub
    Protected Sub GridT_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Gridt.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = Gridt.Rows(e.NewEditIndex)

            lbItemNoT.Text = GVR.Cells(1).Text
            BindToText(tbTrainingCode, GVR.Cells(2).Text)
            BindToText(tbTrainingName, GVR.Cells(3).Text)
            BindToText(tbTrainingPlace, GVR.Cells(4).Text)
            BindToText(tbTutorName, GVR.Cells(5).Text)
            BindToText(tbInstitution, GVR.Cells(6).Text)
            BindToText(tbLocation, GVR.Cells(7).Text)
            BindToText(tbTrainingPeriod, GVR.Cells(8).Text)
            BindToDropList(ddlFgCertificate, GVR.Cells(9).Text)
            BindToText(tbCertificateT, GVR.Cells(10).Text)
            BindToDropList(ddlCostType, GVR.Cells(11).Text)
            BindToText(tbNilai, GVR.Cells(12).Text)
            pnlEditTraining.Visible = True
            pnlTraining.Visible = False
            ViewState("StateDtT") = "Edit"
            tbTrainingCode.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub BindDataDtBank(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtB") = Nothing
            dt = SQLExecuteQuery(GetStringDtBank(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtB") = dt
            Gridb.DataSource = dt
            Gridb.DataBind()
            BindGridDt(dt, Gridb)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Bank Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub GridB_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gridb.PageIndexChanging
        Try
            Gridb.PageIndex = e.NewPageIndex
            Gridb.DataSource = ViewState("DtB")
            Gridb.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub
    Protected Sub GridB_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Gridb.RowDeleting
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = Gridb.Rows(e.RowIndex)
        SQLExecuteNonQuery("Delete from MsEmpBank where EmpNumb = " + QuotedStr(tbCode.Text) + " AND Bank = " + QuotedStr(GVR.Cells(1).Text), ViewState("DBConnection").ToString)
        Gridb.DataSource = ViewState("DtB")
        BindDataDtBank(tbCode.Text)
        'btnaddb.Visible = Gridb.Rows.Count = 0
        lbStatus.Text = MessageDlg("Data Deleted")
        'EnableHd(Gridb.Rows.Count = 0)
    End Sub
    Private Sub cleardtB()
        Try
            ddlBank.SelectedValue = ""
            tbBankAddr1.Text = ""
            tbBankAddr2.Text = ""
            tbBankAccount.Text = ""
            tbBankAccountName.Text = ""
            tbsandi.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Bank Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnaddB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddb.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If

        cleardtB()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtB").select("")
            i = Row.Length
        End If
        If i = 1 Then
            lbStatus.Text = MessageDlg("cannot insert")
            Exit Sub
        End If
        ViewState("StateDtB") = "Insert"

        pnlEditBank.Visible = True
        PnlBank.Visible = False
        EnableHd(False)
        ddlBank.Enabled = True
        ddlBank.Focus()
        Exit Sub
    End Sub
    Protected Sub GridB_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Gridb.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = Gridb.Rows(e.NewEditIndex)

            ddlBank.SelectedValue = GVR.Cells(1).Text
            tbBankAddr1.Text = GVR.Cells(2).Text.Replace("&nbsp;", "")
            tbBankAddr2.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbBankAccount.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbBankAccountName.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbsandi.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            pnlEditBank.Visible = True
            PnlBank.Visible = False
            ViewState("StateDtB") = "Edit"
            ddlBank.Enabled = False
            tbBankAddr1.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SaveDtBank()
        Dim SQLString As String
        Try
            If ddlBank.SelectedValue.Trim = "" Then
                lbStatus.Text = MessageDlg("Bank Must Have Value")
                ddlBank.Focus()
                Exit Sub
            End If
            If tbBankAccount.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Bank Must Have Value")
                tbBankAccount.Focus()
                Exit Sub
            End If

            If ViewState("StateDtB") = "Insert" Then
                If SQLExecuteScalar("SELECT Bank FROM MsEmpBank WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND Bank = " + QuotedStr(ddlBank.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Bank " + QuotedStr(ddlBank.SelectedValue) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpBank (EmpNumb, Bank, BankAddr1, BankAddr2, BankAccountNo, BankAccountName, BankSandi, UserId, UserDate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(ddlBank.SelectedValue) + ", " + QuotedStr(tbBankAddr1.Text) + ", " + QuotedStr(tbBankAddr2.Text) + ", " + QuotedStr(tbBankAccount.Text) + ", " + QuotedStr(tbBankAccountName.Text) + ", " + _
                QuotedStr(tbsandi.Text) + ", " + QuotedStr(ViewState("UserId").ToString) + ", getdate()"

            Else
                SQLString = "UPDATE MsEmpBank SET BankAddr1 = " + QuotedStr(tbBankAddr1.Text) + ", BankAddr2 = " + QuotedStr(tbBankAddr2.Text) + ", " + _
                " BankAccountNo = " + QuotedStr(tbBankAccount.Text) + ", BankAccountName = " + QuotedStr(tbBankAccountName.Text) + ", " + _
                " BankSandi = " + QuotedStr(tbsandi.Text) + ", UserId = " + QuotedStr(ViewState("UserId").ToString) + ", " + _
                " UserDate = Getdate()" + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND Bank = " + QuotedStr(ddlBank.SelectedValue)

            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditBank.Visible = False
            PnlBank.Visible = True
            EnableHd(Gridb.Rows.Count = 0)
            BindDataDtBank(ViewState("EmpNumb"))
            Gridb.DataSource = ViewState("DtB")
            Gridb.DataBind()

            pnlEditBank.Visible = False
            PnlBank.Visible = True
            Gridb.PageIndex = 0
            BindDataDtBank(ViewState("EmpNumb"))
            'btnaddb.Visible = Gridb.Rows.Count = 0
        Catch ex As Exception
            Throw New Exception("Save Bank Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveb.Click
        Try
            SaveDtBank()
           
        Catch ex As Exception
            lbStatus.Text = "Save Bank Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancelb.Click
        Try
            pnlEditBank.Visible = False
            PnlBank.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Bank Error : " + ex.ToString
        End Try
    End Sub

    Private Sub BindDataDtEme(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtEme") = Nothing
            dt = SQLExecuteQuery(GetStringDtEmergency(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtEme") = dt
            Grideme.DataSource = dt
            Grideme.DataBind()
            BindGridDt(dt, Grideme)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub GridEme_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Grideme.PageIndexChanging
        Try
            Grideme.PageIndex = e.NewPageIndex
            Grideme.DataSource = ViewState("DtEme")
            Grideme.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub
    Protected Sub GridEme_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Grideme.RowDeleting
        Dim SQLString As String
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = Grideme.Rows(e.RowIndex)
        SQLString = "Delete from MsEmpEmergency where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(GVR.Cells(1).Text)
        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Grideme.DataSource = ViewState("Dteme")
        BindDataDtEme(tbCode.Text)
        lbStatus.Text = MessageDlg("Data Deleted")
        'EnableHd(GridEme.Rows.Count = 0)
    End Sub
    Private Sub clearDtEme()
        Try
            lbItemNoEmergency.Text = ""
            tbContactNameE.Text = ""
            ddlGenderE.SelectedIndex = 0
            tbRelationship.Text = ""
            tbEmailE.Text = ""
            tbaddress1E.Text = ""
            tbaddress2E.Text = ""
            tbZipCodeE.Text = ""
            tbPhoneE.Text = ""
            tbFax.Text = ""
            tbHP.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Eme Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnaddEme_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddeme.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        clearDtEme()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtEme").select("")
            i = Row.Length
        End If

        ViewState("StateDtEme") = "Insert"
        If i > 0 Then
            lbItemNoEmergency.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNoEmergency.Text = "1"
        End If
        pnlEditEmergency.Visible = True
        pnlEmergency.Visible = False
        EnableHd(False)
        tbContactNameE.Focus()
        Exit Sub
    End Sub
    Protected Sub GridEme_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Grideme.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = Grideme.Rows(e.NewEditIndex)

            lbItemNoEmergency.Text = GVR.Cells(1).Text
            tbContactNameE.Text = GVR.Cells(2).Text
            ddlGenderE.SelectedValue = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbRelationship.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbEmailE.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbaddress1E.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            tbaddress2E.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            tbZipCodeE.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            tbPhoneE.Text = GVR.Cells(9).Text.Replace("&nbsp;", "")
            tbFax.Text = GVR.Cells(10).Text.Replace("&nbsp;", "")
            tbHP.Text = GVR.Cells(11).Text.Replace("&nbsp;", "")
            pnlEditEmergency.Visible = True
            pnlEmergency.Visible = False
            ViewState("StateDtEme") = "Edit"
            tbSkilName.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SaveDtEme()
        Dim SQLString As String
        Try
            If tbContactNameE.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Contact Name Must Have Value")
                tbContactNameE.Focus()
                Exit Sub
            End If
            If tbRelationship.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Relation Ship Must Have Value")
                tbRelationship.Focus()
                Exit Sub
            End If

            If ViewState("StateDtEme") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsEmpEmergency WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNoEmergency.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Emergency " + QuotedStr(lbItemNoEmergency.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpEmergency (EmpNumb, ItemNo, ContactName, Gender, RelationShip, Address1, Address2, ZipCode, Email, Phone, Fax, HandPhone, UserId, UserDate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemNoEmergency.Text + ", " + QuotedStr(tbContactNameE.Text) + ", " + QuotedStr(ddlGenderE.SelectedValue) + ", " + QuotedStr(tbRelationship.Text) + ", " + QuotedStr(tbaddress1E.Text) + ", " + _
                QuotedStr(tbaddress2E.Text) + ", " + QuotedStr(tbZipCodeE.Text) + ", " + QuotedStr(tbEmailE.Text) + ", " + QuotedStr(tbPhoneE.Text) + ", " + QuotedStr(tbFax.Text) + ", " + QuotedStr(tbHP.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", Getdate()"

            Else
                SQLString = "UPDATE MsEmpEmergency SET ContactName = " + QuotedStr(tbContactNameE.Text) + ", Gender = " + QuotedStr(ddlGenderE.SelectedValue) + ", " + _
                " RelationShip = " + QuotedStr(tbRelationship.Text) + ", Address1 = " + QuotedStr(tbaddress1E.Text) + ", " + _
                " Address2 = " + QuotedStr(tbaddress2E.Text) + ", ZipCode = " + QuotedStr(tbZipCodeE.Text) + ", " + _
                " Email = " + QuotedStr(tbEmailE.Text) + ", Phone = " + QuotedStr(tbPhoneE.Text) + ", " + _
                " Fax = " + QuotedStr(tbFax.Text) + ", HandPhone = " + QuotedStr(tbHP.Text) + ", " + _
                " UserID = " + QuotedStr(ViewState("UserId").ToString) + ", UserDate = Getdate()" + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNoEmergency.Text)

            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditEmergency.Visible = False
            pnlEmergency.Visible = True
            EnableHd(Grideme.Rows.Count = 0)
            BindDataDtEme(ViewState("EmpNumb"))
            Grideme.DataSource = ViewState("DtEme")
            Grideme.DataBind()

            pnlEditEmergency.Visible = False
            pnlEmergency.Visible = True
            Grideme.PageIndex = 0
            BindDataDtEme(ViewState("EmpNumb"))
        Catch ex As Exception
            Throw New Exception("Save Eme Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveeme_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveeme.Click
        Try
            SaveDtEme()
            
        Catch ex As Exception
            lbStatus.Text = "Save eme Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCanceleme_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncanceleme.Click
        Try
            pnlEditEmergency.Visible = False
            pnlEmergency.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub BindDataDtSO(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtSO") = Nothing
            dt = SQLExecuteQuery(GetStringDtSociety(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtSO") = dt
            Gridso.DataSource = dt
            Gridso.DataBind()
            BindGridDt(dt, Gridso)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub GridSO_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gridso.PageIndexChanging
        Try
            Gridso.PageIndex = e.NewPageIndex
            Gridso.DataSource = ViewState("DtSO")
            Gridso.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub
    Protected Sub GridSO_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Gridso.RowDeleting
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = Gridso.Rows(e.RowIndex)
        SQLExecuteNonQuery("Delete from MsEmpSociety where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + GVR.Cells(1).Text, ViewState("DBConnection").ToString)
        BindDataDtSO(tbCode.Text)
        Gridso.DataSource = ViewState("DtSO")

        lbStatus.Text = MessageDlg("Data Deleted")
        'EnableHd(GridSO.Rows.Count = 0)
    End Sub
    Private Sub clearDtSO()
        Try
            lbItemNoS.Text = ""
            tbSociety.Text = ""
            tbJobInfo.Text = ""
            tbJobTitle.Text = ""
            tbYear.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Eme Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnaddso_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddso.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        clearDtSO()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtSO").select("")
            i = Row.Length
        End If

        ViewState("StateDtSO") = "Insert"
        If i > 0 Then
            lbItemNoS.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNoS.Text = "1"
        End If
        pnlEditSociety.Visible = True
        pnlSociety.Visible = False
        EnableHd(False)
        tbSociety.Focus()
        Exit Sub
    End Sub
    Protected Sub GridSO_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Gridso.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = Gridso.Rows(e.NewEditIndex)

            lbItemNoS.Text = GVR.Cells(1).Text
            tbSociety.Text = GVR.Cells(2).Text
            tbJobInfo.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbJobTitle.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbYear.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            pnlEditSociety.Visible = True
            pnlSociety.Visible = False
            ViewState("StateDtSO") = "Edit"
            tbSociety.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SaveDtSO()
        Dim SQLString As String
        Try
            If tbSociety.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Society Name Must Have Value")
                tbSociety.Focus()
                Exit Sub
            End If

            If ViewState("StateDtSO") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsEmpSociety WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNoS.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Society " + QuotedStr(lbItemNoS.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpSociety (EmpNumb, ItemNo, SocietyName, JobInfo, JobTitle, OccupationYear, UserId, UserDate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemNoS.Text + ", " + QuotedStr(tbSociety.Text) + ", " + QuotedStr(tbJobInfo.Text) + ", " + QuotedStr(tbJobTitle.Text) + ", " + QuotedStr(tbYear.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", Getdate()"

            Else
                SQLString = "UPDATE MsEmpSociety SET SocietyName = " + QuotedStr(tbSociety.Text) + ", JobInfo = " + QuotedStr(tbJobInfo.Text) + ", " + _
                " JobTitle = " + QuotedStr(tbJobTitle.Text) + ", OccupationYear = " + QuotedStr(tbYear.Text) + ", " + _
                " UserID = " + QuotedStr(ViewState("UserId").ToString) + ", UserDate = Getdate()" + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNoS.Text)

            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditSociety.Visible = False
            pnlSociety.Visible = True
            EnableHd(Gridso.Rows.Count = 0)
            BindDataDtSO(ViewState("EmpNumb"))
            Gridso.DataSource = ViewState("DtSO")
            Gridso.DataBind()

            pnlEditSociety.Visible = False
            pnlSociety.Visible = True
            Gridso.PageIndex = 0
            BindDataDtSO(ViewState("EmpNumb"))
        Catch ex As Exception
            Throw New Exception("Save Society Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveSo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveso.Click
        Try
            SaveDtSO()
           
        Catch ex As Exception
            lbStatus.Text = "Save Society Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelSO_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancelso.Click
        Try
            pnlEditSociety.Visible = False
            pnlSociety.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub BindDataDtH(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtH") = Nothing
            dt = SQLExecuteQuery(GetStringDtHobby(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtH") = dt
            Gridh.DataSource = dt
            Gridh.DataBind()
            BindGridDt(dt, Gridh)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub GridH_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gridh.PageIndexChanging
        Try
            Gridh.PageIndex = e.NewPageIndex
            Gridh.DataSource = ViewState("DtH")
            Gridh.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub
    Protected Sub GridH_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Gridh.RowDeleting

        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = Gridh.Rows(e.RowIndex)
        SQLExecuteNonQuery("Delete from MsEmpHobby where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + GVR.Cells(1).Text, ViewState("DBConnection").ToString)
        BindDataDtH(tbCode.Text)
        Gridh.DataSource = ViewState("Dth")

        lbStatus.Text = MessageDlg("Data Deleted")
        'EnableHd(GridH.Rows.Count = 0)
    End Sub
    Private Sub clearDtH()
        Try
            lbItemNoHobby.Text = ""
            tbHobby.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear H Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnaddH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddh.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        clearDtH()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtH").select("")
            i = Row.Length
        End If

        ViewState("StateDtH") = "Insert"
        If i > 0 Then
            lbItemNoHobby.Text = (CInt(Row(i - 1)("ItemNo").ToString) + 1).ToString
        Else
            lbItemNoHobby.Text = "1"
        End If
        pnlEditHobby.Visible = True
        pnlHobby.Visible = False
        EnableHd(False)
        tbHobby.Focus()
        Exit Sub
    End Sub
    Protected Sub GridH_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Gridh.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = Gridh.Rows(e.NewEditIndex)

            lbItemNoHobby.Text = GVR.Cells(1).Text
            tbHobby.Text = GVR.Cells(2).Text
            pnlEditHobby.Visible = True
            pnlHobby.Visible = False
            ViewState("StateDtH") = "Edit"
            tbSkilName.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SaveDtH()
        Dim SQLString As String
        Try
            If tbHobby.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Hobby Must Have Value")
                tbHobby.Focus()
                Exit Sub
            End If

            If ViewState("StateDtH") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsEmpHobby WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNoHobby.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Hobby " + QuotedStr(lbItemNoHobby.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpHobby (EmpNumb, ItemNo, Hobby, UserId, UserDate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemNoHobby.Text + ", " + QuotedStr(tbHobby.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", Getdate()"

            Else
                SQLString = "UPDATE MsEmpHobby SET Hobby = " + QuotedStr(tbHobby.Text) + _
                ", UserID = " + QuotedStr(ViewState("UserId").ToString) + ", UserDate = Getdate()" + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNoHobby.Text)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditHobby.Visible = False
            pnlHobby.Visible = True
            EnableHd(Gridh.Rows.Count = 0)
            BindDataDtH(ViewState("EmpNumb"))
            Gridh.DataSource = ViewState("DtH")
            Gridh.DataBind()

            pnlEditHobby.Visible = False
            pnlHobby.Visible = True
            Gridh.PageIndex = 0
            BindDataDtH(ViewState("EmpNumb"))
        Catch ex As Exception
            Throw New Exception("Save Memo Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveH_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveH.Click
        Try
            SaveDtH()
            
        Catch ex As Exception
            lbStatus.Text = "Save hobby Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelH_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancelH.Click
        Try
            pnlEditHobby.Visible = False
            pnlHobby.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub


    Private Sub BindDataDtM(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtM") = Nothing
            dt = SQLExecuteQuery(GetStringDtMemo(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtM") = dt
            GridM.DataSource = dt
            GridM.DataBind()
            BindGridDt(dt, GridM)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub GridM_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridM.PageIndexChanging
        Try
            GridM.PageIndex = e.NewPageIndex
            GridM.DataSource = ViewState("DtM")
            GridM.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub
    Protected Sub GridM_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridM.RowDeleting
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridM.Rows(e.RowIndex)
        SQLExecuteNonQuery("Delete from MsEmpMemo where EmpNumb = " + QuotedStr(tbCode.Text) + " AND ReferenceBy = " + QuotedStr(GVR.Cells(1).Text), ViewState("DBConnection").ToString)
        BindDataDtM(tbCode.Text)
        GridM.DataSource = ViewState("DtM")
        lbStatus.Text = MessageDlg("Data Deleted")
    End Sub
    Private Sub clearDtM()
        Try
            tbReferenceBy.Text = ""
            tbReferenceAddr1.Text = ""
            tbReferenceAddr2.Text = ""
            tbReferenceJob.Text = ""
            tbReferencePhone.Text = ""
            tbMemo.Text = ""
            tbMemo2.Text = ""
            tbMemo3.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear H Error " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnaddM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddM.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        clearDtM()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = MessageDlg("Employee Must Filled")
            tbCode.Focus()
            Exit Sub
        End If
        Dim i As Integer
        Dim dt As New DataTable

        Dim Row As DataRow()
        If ViewState("StateHd") = "Insert" Then
            Row = Nothing
            i = 0
        Else
            Row = ViewState("DtM").select("")
            i = Row.Length
        End If
        If i = 1 Then
            lbStatus.Text = MessageDlg("cannot insert")
            Exit Sub
        End If
        ViewState("StateDtM") = "Insert"
        tbReferenceBy.Enabled = True
        pnlEditMemo.Visible = True
        pnlMemo.Visible = False
        EnableHd(False)
        tbReferenceBy.Focus()
        Exit Sub
    End Sub
    Protected Sub GridM_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridM.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridM.Rows(e.NewEditIndex)
            tbReferenceBy.Enabled = False
            tbReferenceBy.Text = GVR.Cells(1).Text.Replace("&nbsp;", "")
            tbReferenceAddr1.Text = GVR.Cells(2).Text.Replace("&nbsp;", "")
            tbReferenceAddr2.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbReferenceJob.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbReferencePhone.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbMemo.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            tbMemo2.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            tbMemo3.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            pnlEditMemo.Visible = True
            pnlMemo.Visible = False
            ViewState("StateDtM") = "Edit"
            tbSkilName.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Private Sub SaveDtM()
        Dim SQLString As String
        Try
            If ViewState("StateDtM") = "Insert" Then
                If SQLExecuteScalar("SELECT ReferenceBy FROM MsEmpMemo WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ReferenceBy = " + QuotedStr(tbReferenceBy.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = MessageDlg("Detail Memo " + QuotedStr(tbReferenceBy.Text) + " has already been exist")
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsEmpMemo (EmpNumb, ReferenceBy, ReferenceAddr1, ReferenceAddr2, ReferenceJob, ReferencePhone, Memo1, Memo2, Memo3) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbReferenceBy.Text) + ", " + QuotedStr(tbReferenceAddr1.Text) + ", " + QuotedStr(tbReferenceAddr2.Text) + ", " + QuotedStr(tbReferenceJob.Text) + ", " + QuotedStr(tbReferencePhone.Text) + ", " + _
                QuotedStr(tbMemo.Text) + ", " + QuotedStr(tbMemo2.Text) + ", " + QuotedStr(tbMemo3.Text)
            Else
                SQLString = "UPDATE MsEmpMemo SET ReferenceBy = " + QuotedStr(tbReferenceBy.Text) + ", ReferenceAddr1 = " + QuotedStr(tbReferenceAddr1.Text) + ", " + _
                " ReferenceAddr2 = " + QuotedStr(tbReferenceAddr2.Text) + ", ReferenceJob = " + QuotedStr(tbReferenceJob.Text) + ", " + _
                " ReferencePhone = " + QuotedStr(tbReferencePhone.Text) + ", Memo1 = " + QuotedStr(tbMemo.Text) + ", " + _
                " Memo2 = " + QuotedStr(tbMemo2.Text) + ", Memo3 = " + QuotedStr(tbMemo3.Text) + _
                " WHERE EmpNumb = " + QuotedStr(tbCode.Text) + " AND ReferenceBy = " + QuotedStr(tbReferenceBy.Text)
            End If
            SQLString = ChangeQuoteNull(SQLString)
            SQLString = SQLString.Replace("'1900-01-01'", "NULL")
            SQLString = SQLString.Replace("'0001-01-01'", "NULL")
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditMemo.Visible = False
            pnlMemo.Visible = True
            EnableHd(GridM.Rows.Count = 0)
            BindDataDtM(ViewState("EmpNumb"))
            GridM.DataSource = ViewState("DtM")
            GridM.DataBind()

        Catch ex As Exception
            Throw New Exception("Save H Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSaveM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsavem.Click
        Try
            SaveDtM()
            pnlEditMemo.Visible = False
            pnlMemo.Visible = True
            GridM.PageIndex = 0
            BindDataDtM(ViewState("EmpNumb"))
        Catch ex As Exception
            lbStatus.Text = "Save Memo Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub btnCancelM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancelm.Click
        Try
            pnlEditMemo.Visible = False
            pnlMemo.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Private Sub clearT()
        Try
            lbItemNoT.Text = ""
            tbTrainingCode.Text = ""
            tbTrainingName.Text = ""
            tbTrainingPlace.Text = ""
            tbTrainingPeriod.Text = ""
            tbInstitution.Text = ""
            tbTutorName.Text = ""
            tbLocation.Text = ""
            tbCertificateT.Text = ""
            ddlFgCertificate.SelectedIndex = 1
            ddlCostType.SelectedIndex = 0
            tbNilai.Text = "0"
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    
    Protected Sub ddlFgJamsosTek_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgJamsosTek.SelectedIndexChanged
        If ddlFgJamsosTek.SelectedIndex = 1 Then
            tbjamsostek.Text = ""
            tbJamsostekDate.SelectedValue = ""
            tbjamsostek.Enabled = False
            tbJamsostekDate.Enabled = False
        Else
            tbjamsostek.Text = ""
            tbJamsostekDate.SelectedValue = ""
            tbjamsostek.Enabled = True
            tbJamsostekDate.Enabled = True
        End If
    End Sub

    Protected Sub btnTraining_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTraining.Click
        Dim ResultField As String
        Try
            Session("filter") = "Select TrainingCode, TrainingName From MsTraining"
            Session("DBConnection") = ViewState("DBConnection")
            ResultField = "TrainingCode, TrainingName"
            ViewState("Sender") = "btnTraining"
            Session("Column") = ResultField.Split(",")
            AttachScript("OpenSearchDlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Search supp Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTrainingCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTrainingCode.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Training", tbTrainingCode.Text, ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                tbTrainingCode.Text = Dr("TrainingCode")
                tbTrainingName.Text = Dr("TrainingName")
                tbTrainingPlace.Focus()
            Else
                tbTrainingCode.Text = ""
                tbTrainingName.Text = ""
                tbTrainingCode.Focus()
            End If

        Catch ex As Exception
            Throw New Exception("tb Trainnig change Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub GridDt_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridDt.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub Gridf_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Gridf.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub Grids_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridS.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub GridEdu_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridEdu.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub GridExp_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridExp.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub GridT_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Gridt.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub Gridp_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridP.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub Gridb_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Gridb.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub Grideme_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Grideme.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub Gridso_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Gridso.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub GridH_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Gridh.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub GridM_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridM.Sorting
        Try
            If ViewState("SortOrder") = Nothing Or ViewState("SortOrder") = "DESC" Then
                ViewState("SortOrder") = "ASC"
            Else
                ViewState("SortOrder") = "DESC"
            End If
            ViewState("SortExpression") = e.SortExpression + " " + ViewState("SortOrder")

        Catch ex As Exception
            lbStatus.Text = "UserDG_SortCommandError =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub tbsalaryExp_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbsalaryExp.TextChanged
        Try
            If tbsalaryExp.Text = "" Then
                tbsalaryExp.Text = "0"
            End If
            tbsalaryExp.Text = FormatNumber(tbsalaryExp.Text, ViewState("DigitCurr"))
        Catch ex As Exception
            lbStatus.Text = "tbsalaryExp_TextChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub tbGPA_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbGPA.TextChanged
        Try
            If tbGPA.Text = "" Then
                tbGPA.Text = "0"
            End If
            tbGPA.Text = FormatNumber(tbGPA.Text, ViewState("DigitCurr"))
        Catch ex As Exception
            lbStatus.Text = "tbsalaryExp_TextChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub ddlGraduated_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGraduated.SelectedIndexChanged
        Try
            If ddlGraduated.Text = "Y" Then
                tbGraduated.Enabled = True
            Else
                tbGraduated.Enabled = False
            End If
            tbGraduated.Text = "0"
        Catch ex As Exception
            lbStatus.Text = "ddlGraduated_SelectedIndexChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub

    'Protected Sub ddlSubSection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubSection.SelectedIndexChanged
    '    Dim Dr As DataRow
    '    Try

    '        Dr = FindMaster("SubSection", ddlSubSection.SelectedValue, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            'BindToDropList(ddlSubSection, Dr("Sub_Section_Code"))
    '            BindToDropList(ddlsection, Dr("Section_Code"))
    '            BindToDropList(ddlDepartment, Dr("Dept_Code"))
    '            'If Dr("Section_Code").ToString = "" Or ddlsection.SelectedValue <> "" Then
    '            '    ddlsection.SelectedValue = ""
    '            'ElseIf Dr("Dept_Code").ToString = "" Or ddlDepartment.SelectedValue <> "" Then
    '            '    ddlDepartment.SelectedValue = ""
    '            'End If
    '            ddlEmpStatus.Focus()
    '        End If

    '    Catch ex As Exception
    '        Throw New Exception("ddlSubSection_SelectedIndexChanged Error : " + ex.ToString)
    '    End Try
    'End Sub
    
    'Protected Sub ddlsection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlsection.SelectedIndexChanged
    '    Dim Dr As DataRow
    '    Try

    '        Dr = FindMaster("Section", ddlSubSection.SelectedValue, ViewState("DBConnection").ToString)
    '        If Not Dr Is Nothing Then
    '            'BindToDropList(ddlSubSection, Dr("Sub_Section_Code"))
    '            BindToDropList(ddlsection, Dr("Section_Code"))
    '            BindToDropList(ddlDepartment, Dr("Dept_Code"))
    '            'If Dr("Section_Code").ToString = "" Or ddlsection.SelectedValue <> "" Then
    '            '    ddlsection.SelectedValue = ""
    '            'ElseIf Dr("Dept_Code").ToString = "" Or ddlDepartment.SelectedValue <> "" Then
    '            '    ddlDepartment.SelectedValue = ""
    '            'End If
    '            ddlEmpStatus.Focus()
    '        End If

    '    Catch ex As Exception
    '        Throw New Exception("ddlsection_SelectedIndexChanged Error : " + ex.ToString)
    '    End Try
    'End Sub

    Protected Sub btnCandidate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCandidate.Click
        Dim FieldResult As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("filter") = "SELECT * From V_PECandidateForMsEmployee "
            FieldResult = "Candidate_No, Candidate_Name, Gender, Birth_Place, Birth_Date, Height, Weight, Res_Addr1, Res_Addr2, Rest_Zip_Code, Res_Phone, Hand_Phone, Ori_Addr1, Ori_Addr2, Ori_Zip_Code, Ori_Phone, Religion, MaritalSt, Type_Card, ID_Card, Job_Title_Code, Emp_Status_Code, Email, Res_City, Ori_City"
            Session("Column") = FieldResult.Split(",")
            AttachScript("OpenPopup();", Page, Me.GetType())
            ViewState("Sender") = "btnCandidate"
        Catch ex As Exception
            lbStatus.Text = "btnAccInvent Error : " + ex.ToString
        End Try
    End Sub

    Protected Friend Sub tbCandidate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCandidate.TextChanged
        Dim Dr As DataRow
        Try
            Dr = FindMaster("CandidateForMsEmployee", QuotedStr(tbCandidate.Text), ViewState("DBConnection").ToString)
            If Not Dr Is Nothing Then
                BindToText(tbCandidate, Dr("Candidate_No").ToString)
                BindToDropList(ddlGender, Dr("Gender").ToString)
                BindToText(tbBirthPlace, Dr("Birth_Place").ToString)
                BindToDate(tbBirthDate, Dr("Birth_Date").ToString)
                BindToText(tbWeight, Dr("Weight").ToString)
                BindToText(tbHeight, Dr("Height").ToString)
                BindToDropList(ddlReligion, Dr("Religion").ToString)
                BindToDropList(ddlTypeCard, Dr("Type_Card").ToString)
                BindToText(tbIDCard, Dr("Id_Card").ToString)
                BindToDropList(ddlJobTitle, Dr("Job_Title_Code").ToString)
                BindToDropList(ddlEmpStatus, Dr("Emp_Status_Code").ToString)
                BindToDropList(ddlMaritalSt, Dr("MaritalSt").ToString)
                BindToText(tbResAddr1, Dr("Res_Addr1").ToString)
                BindToText(tbResAddr2, Dr("Res_Addr2").ToString)
                BindToText(tbResZipCode, Dr("Rest_Zip_Code").ToString)
                BindToText(tbOriAddr1, Dr("Ori_Addr1").ToString)
                BindToText(tbOriAddr2, Dr("Ori_Addr2").ToString)
                BindToText(tbOriZipCode, Dr("Ori_Zip_Code").ToString)
                BindToText(tbResPhone, Dr("Res_Phone").ToString)
                BindToText(tbOriPhone, Dr("Ori_Phone").ToString)
                BindToText(tbEmail, Dr("Email").ToString)

                'BindToDropList(ddlResAddrStatus, Dr("Res_AddrStatus").ToString)
                'BindToDropList(ddlStatus, Dr("TKAStatus").ToString)
                'BindToText(tbLastNo, Dr("LastCertificateNo").ToString)
                'BindToDate(tbStartDate, Dr("Hire_Date").ToString)
                'BindToDate(tbEndDate, Dr("End_Date").ToString)
                'BindToText(tbPhoneContact, Dr("HandPhone").ToString)
                'BindToDropList(ddlMaritalTax, Dr("MaritalTax").ToString)
                'BindToDropList(ddlActive, Dr("Fg_Active").ToString)
                'BindToDropList(ddlSalaryType, Dr("Salary_Type").ToString)
                'BindToText(tbNPWP, Dr("NPWP").ToString)
                'BindToText(tbCard, Dr("AbsenceCard").ToString)
                'BindToDropList(ddlJobLevel, Dr("Job_Level_Code").ToString)
                'BindToDropList(ddlWorkPlace, Dr("Work_Place_Code").ToString)
                ''BindToDropList(ddlSubSection, Dr("Sub_Section").ToString)
                'BindToDropList(ddlDepartment, Dr("Dept_Code").ToString)
                'BindToDropList(ddlTKA, Dr("FgTKA").ToString)
                'BindToDropList(ddlFgJamsosTek, Dr("FgJamsosTek").ToString)
                'BindToDropList(ddlResCity, Dr("Res_City").ToString)
                'BindToDropList(ddlOriCity, Dr("Ori_City").ToString)
                'BindToText(tbSKNo, Dr("SKNo").ToString)
                'BindToText(tbAKDHK, Dr("AKDHKNo").ToString)
                'BindToText(tbPinBB, Dr("PINBB").ToString)
                'BindToText(tbjamsostek, Dr("JamSosTekNo").ToString)
                'BindToText(tbMaritalDocNo, Dr("MaritalDocNo").ToString)
                'BindToDate(tbJamsostekDate, Dr("JamSosTekDate").ToString)
                'BindToDate(tbDateMarital, Dr("MaritalDate").ToString)
                'BindToDate(tbEndContract, Dr("End_Date_Contract").ToString)
                ddlBloodType.Focus()
            End If
            
            
            'End If
        Catch ex As Exception
            lbStatus.Text = "tbCandidate_TextChanged Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlEmpStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEmpStatus.SelectedIndexChanged
        Dim FgPermanent As String
        Try
            FgPermanent = SQLExecuteScalar("Select FgPermanent from msEmpStatus Where EmpStatusCode = " + QuotedStr(ddlEmpStatus.SelectedValue), ViewState("DBConnection"))
            If FgPermanent = "Y" Then
                tbEndContract.Clear()
            End If
        Catch ex As Exception
            lbStatus.Text = "ddlEmpStatus_SelectedIndexChanged Error : " + ex.ToString
        End Try
    End Sub
End Class
