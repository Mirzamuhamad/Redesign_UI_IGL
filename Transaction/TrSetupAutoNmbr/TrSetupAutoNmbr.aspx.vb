Imports System.Data

Partial Class Transaction_TrSetupAutoNmbr_TrSetupAutoNmbr
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            bindDataGrid()

        End If
        lstatus.Text = ""
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

    Private Sub bindDataGrid()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("SELECT A.SetCode, A.SetDescription, A.SetValue FROM MsSetup A WHERE SetGroup = 'AutoNmbr' ORDER BY A.SetDescription ", ViewState("DBConnection").ToString)
            DV = tempDS.Tables(0).DefaultView

            DV.Sort = ViewState("SortExpression")
            DataGrid.DataSource = DV
            DataGrid.DataBind()
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    'Public Sub ItemCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    '    Try
    '        If e.CommandName = "Edit" Then
    '            PnlMain.Visible = False
    '            pnlInput.Visible = True
    '            Dim lbDesc, lbCode, lbValue As Label
    '            lbDesc = e.Item.FindControl("SetDescription")
    '            lbCode = e.Item.FindControl("SetCode")
    '            lbValue = e.Item.FindControl("SetValue")

    '            lbCurrentFormat.Text = lbValue.Text
    '            lbNewFormat.Text = lbValue.Text
    '            tbDescription.Text = lbDesc.Text
    '            lbExample.Text = GenerateSample(lbNewFormat.Text)
    '            ViewState("SetCode") = lbCode.Text
    '            ResetField()
    '            Disintegrator(lbValue.Text)
    '            PnlMain.Visible = False
    '            pnlInput.Visible = True
    '        End If
    '    Catch ex As Exception
    '        lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
    '    End Try
    'End Sub

    Public Sub Disintegrator(ByVal Input As String)
        Dim CurrInput As String
        Dim I, Index, Priority, CounterYear As Integer
        Dim Comp, Trans, AddParam, Taon, Bulan, digit As Boolean
        Try
            CurrInput = ""
            Index = 0
            Priority = 1
            Comp = False
            Trans = False
            AddParam = False
            Taon = False
            Bulan = False
            digit = False
            CounterYear = 0

            For I = 0 To Input.Length - 1
                Select Case Input(I)
                    Case "+"
                        If CurrInput <> "Company" And CurrInput <> "" Then
                            modifySeparator(CurrInput, "")
                        End If
                        If CurrInput <> "Company" Then
                            cxCompany.Checked = True
                            CurrInput = "Company"
                            ddlPriority1.SelectedValue = Priority.ToString
                            Priority = Priority + 1
                            Comp = True
                        End If
                    Case "/"
                        modifySeparator(CurrInput, Input(I))
                        CurrInput = ""
                    Case "-"
                        modifySeparator(CurrInput, Input(I))
                        CurrInput = ""
                    Case "."
                        modifySeparator(CurrInput, Input(I))
                        CurrInput = ""
                    Case "@"
                        If CurrInput <> "AddParam" And CurrInput <> "" Then
                            modifySeparator(CurrInput, "")
                        End If
                        If CurrInput <> "AddParam" Then
                            cxAddParam.Checked = True
                            CurrInput = "AddParam"
                            ddlPriority3.SelectedValue = Priority.ToString
                            Priority = Priority + 1
                            ddlDigitParam.SelectedIndex = 0
                            AddParam = True
                        Else
                            ddlDigitParam.SelectedIndex = ddlDigitParam.SelectedIndex + 1
                        End If
                    Case "$"
                        CounterYear = CounterYear + 1
                        If CounterYear = 4 Then
                            ddlDigitYear.SelectedValue = "4"
                        End If
                        If CurrInput <> "Year" And CurrInput <> "" Then
                            modifySeparator(CurrInput, "")
                        End If
                        If CurrInput <> "Year" Then
                            cxYear.Checked = True
                            CurrInput = "Year"
                            ddlPriority4.SelectedValue = Priority.ToString
                            Priority = Priority + 1
                            ddlDigitYear.SelectedValue = "2"
                            Taon = True
                        End If
                    Case "!"
                        If CurrInput <> "Month" And CurrInput <> "" Then
                            modifySeparator(CurrInput, "")
                        End If
                        If CurrInput <> "Month" Then
                            cxMonth.Checked = True
                            cxByYear.Checked = False
                            cxByYear.Enabled = True
                            CurrInput = "Month"
                            ddlPriority5.SelectedValue = Priority.ToString
                            Priority = Priority + 1
                            Bulan = True
                            ddlDigitMonth.SelectedIndex = 0
                        Else
                            ddlDigitMonth.SelectedIndex = ddlDigitMonth.SelectedIndex + 1
                        End If
                    Case "*" ' generate number by year
                        If CurrInput <> "Month" And CurrInput <> "" Then
                            modifySeparator(CurrInput, "")
                        End If
                        If CurrInput <> "Month" Then
                            cxMonth.Checked = True
                            cxByYear.Enabled = True
                            cxByYear.Checked = True
                            CurrInput = "Month"
                            ddlPriority5.SelectedValue = Priority.ToString
                            Priority = Priority + 1
                            Bulan = True
                            ddlDigitMonth.SelectedIndex = 0
                        Else
                            ddlDigitMonth.SelectedIndex = ddlDigitMonth.SelectedIndex + 1
                        End If
                    Case "#"
                        If CurrInput <> "Digit" And CurrInput <> "" Then
                            modifySeparator(CurrInput, "")
                        End If
                        If CurrInput <> "Digit" Then
                            cxYear.Checked = True
                            CurrInput = "Digit"
                            ddlPriority6.SelectedValue = Priority.ToString
                            Priority = Priority + 1
                            ddlNumber.SelectedIndex = 0
                            digit = True
                        Else
                            ddlNumber.SelectedIndex = ddlNumber.SelectedIndex + 1
                        End If
                    Case Else
                        If Input(I) <> "&" Then
                            tbTransType.Text = tbTransType.Text + Input(I)
                        End If

                        If CurrInput <> "Trans" And CurrInput <> "" Then
                            modifySeparator(CurrInput, "")
                        End If
                        If CurrInput <> "Trans" Then
                            cxTransaction.Checked = True
                            CurrInput = "Trans"
                            ddlPriority2.SelectedValue = Priority.ToString
                            Priority = Priority + 1
                            Trans = True
                        End If
                End Select
            Next

            ddlPriority1.Enabled = Comp
            ddlSeparator1.Enabled = Comp
            If Comp Then
                ViewState("Priority1") = ddlPriority1.SelectedValue
            Else
                ViewState("Priority1") = "0"
            End If


            ddlPriority2.Enabled = Trans
            ddlSeparator2.Enabled = Trans
            tbTransType.Enabled = Trans
            If Trans Then
                ViewState("Priority2") = ddlPriority2.SelectedValue
            Else
                ViewState("Priority2") = "0"
            End If


            ddlPriority3.Enabled = AddParam
            ddlSeparator3.Enabled = AddParam
            ddlDigitParam.Enabled = AddParam
            If AddParam Then
                ViewState("Priority3") = ddlPriority3.SelectedValue
            Else
                ViewState("Priority3") = "0"
            End If

            ddlPriority4.Enabled = Taon
            ddlSeparator4.Enabled = Taon
            ddlDigitYear.Enabled = Taon
            If Taon Then
                ViewState("Priority4") = ddlPriority4.SelectedValue
            Else
                ViewState("Priority4") = "0"
            End If

            ddlPriority5.Enabled = Bulan
            ddlSeparator5.Enabled = Bulan
            ddlDigitMonth.Enabled = Bulan
            If Bulan Then
                ViewState("Priority5") = ddlPriority5.SelectedValue
            Else
                ViewState("Priority5") = "0"
            End If


            ddlPriority6.Enabled = digit
            ddlSeparator6.Enabled = digit
            ddlNumber.Enabled = digit
            ViewState("Priority6") = ddlPriority6.SelectedValue

        Catch ex As Exception
            Throw New Exception("Disintegrator Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub ResetField()
        Try
            ddlDigitMonth.SelectedIndex = 0
            ddlDigitParam.SelectedIndex = 0
            ddlDigitYear.SelectedIndex = 0
            ddlNumber.SelectedIndex = 0
            ddlPriority1.SelectedIndex = 0
            ddlPriority2.SelectedIndex = 0
            ddlPriority3.SelectedIndex = 0
            ddlPriority4.SelectedIndex = 0
            ddlPriority5.SelectedIndex = 0
            ddlPriority6.SelectedIndex = 0
            ddlSeparator1.SelectedIndex = 0
            ddlSeparator2.SelectedIndex = 0
            ddlSeparator3.SelectedIndex = 0
            ddlSeparator4.SelectedIndex = 0
            ddlSeparator5.SelectedIndex = 0
            ddlSeparator6.SelectedIndex = 0
            tbTransType.Text = ""
            cxCompany.Checked = False
            cxTransaction.Checked = False
            cxAddParam.Checked = False
            cxYear.Checked = False
            cxMonth.Checked = False
            cxByYear.Enabled = False
        Catch ex As Exception
            Throw New Exception("Reset Field Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub modifySeparator(ByVal CurrInput As String, ByVal Value As String)
        Try
            If Value = "" Then
                Select Case CurrInput
                    Case "Trans"
                        ddlSeparator2.SelectedIndex = 0
                    Case "AddParam"
                        ddlSeparator3.SelectedIndex = 0
                    Case "Year"
                        ddlSeparator4.SelectedIndex = 0
                    Case "Month"
                        ddlSeparator5.SelectedIndex = 0
                    Case "Digit"
                        ddlSeparator6.SelectedIndex = 0
                    Case "Company"
                        ddlSeparator1.SelectedIndex = 0
                End Select
            Else
                Select Case CurrInput
                    Case "Trans"
                        ddlSeparator2.SelectedValue = Value
                    Case "AddParam"
                        ddlSeparator3.SelectedValue = Value
                    Case "Year"
                        ddlSeparator4.SelectedValue = Value
                    Case "Month"
                        ddlSeparator5.SelectedValue = Value
                    Case "Digit"
                        ddlSeparator6.SelectedValue = Value
                    Case "Company"
                        ddlSeparator1.SelectedValue = Value
                End Select
            End If
        Catch ex As Exception
            Throw New Exception("Modify Separator Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            PnlMain.Visible = True
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String
        Try
            SQLString = "Update MsSetup set SetValue= " + QuotedStr(lbNewFormat.Text) + ", SetDescription = " + QuotedStr(tbDescription.Text) + " WHERE SetGroup = 'AutoNmbr' AND SetCode =" + QuotedStr(ViewState("SetCode"))

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            PnlMain.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cxCompany_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cxCompany.CheckedChanged
        Try
            ddlPriority1.Enabled = cxCompany.Checked
            ddlSeparator1.Enabled = cxCompany.Checked
            If cxCompany.Checked Then
                ddlPriority1.SelectedValue = cekMaxPriority().ToString
                ViewState("Priority1") = ddlPriority1.SelectedValue
                GenerateCode()
            Else
                ChangePriority("Company", "0", ddlPriority1.SelectedValue)
                ddlPriority1.SelectedIndex = 0
                modifySeparator("Company", "")
            End If
        Catch ex As Exception
            lstatus.Text = "cx Company Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cxTransaction_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cxTransaction.CheckedChanged
        Try
            ddlPriority2.Enabled = cxTransaction.Checked
            ddlSeparator2.Enabled = cxTransaction.Checked
            tbTransType.Enabled = cxTransaction.Checked
            If cxTransaction.Checked Then
                ddlPriority2.SelectedValue = cekMaxPriority().ToString
                ViewState("Priority2") = ddlPriority2.SelectedValue
                GenerateCode()
            Else
                ChangePriority("Trans", "0", ddlPriority2.SelectedValue)
                ddlPriority2.SelectedIndex = 0
                modifySeparator("Trans", "")
            End If
            'ddlPriority2_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            lstatus.Text = "cxTransaction Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub cxAddParam_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cxAddParam.CheckedChanged
        Try
            ddlPriority3.Enabled = cxAddParam.Checked
            ddlSeparator3.Enabled = cxAddParam.Checked
            ddlDigitParam.Enabled = cxAddParam.Checked
            If cxAddParam.Checked Then
                ddlPriority3.SelectedValue = cekMaxPriority().ToString
                ViewState("Priority3") = ddlPriority3.SelectedValue
                GenerateCode()
            Else
                ChangePriority("AddParam", "0", ddlPriority3.SelectedValue)
                ddlPriority3.SelectedIndex = 0
                modifySeparator("AddParam", "")
            End If
            'ddlPriority3_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            lstatus.Text = "cxAddParam Checked Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub cxYear_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cxYear.CheckedChanged
        Try
            ddlPriority4.Enabled = cxYear.Checked
            ddlSeparator4.Enabled = cxYear.Checked
            ddlDigitYear.Enabled = cxYear.Checked
            If cxYear.Checked Then
                ddlPriority4.SelectedValue = cekMaxPriority().ToString
                ViewState("Priority4") = ddlPriority4.SelectedValue
                GenerateCode()
            Else
                ChangePriority("Year", "0", ddlPriority4.SelectedValue)
                ddlPriority4.SelectedIndex = 0
                modifySeparator("Year", "")
            End If
            'ddlPriority4_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            lstatus.Text = "cx Year Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub cxMonth_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cxMonth.CheckedChanged
        Try
            ddlPriority5.Enabled = cxMonth.Checked
            ddlSeparator5.Enabled = cxMonth.Checked
            ddlDigitMonth.Enabled = cxMonth.Checked
            cxByYear.Enabled = cxMonth.Checked
            If cxMonth.Checked Then
                ddlPriority5.SelectedValue = cekMaxPriority().ToString
                ViewState("Priority5") = ddlPriority5.SelectedValue
                GenerateCode()
            Else
                ChangePriority("Month", "0", ddlPriority5.SelectedValue)
                ddlPriority5.SelectedIndex = 0
                modifySeparator("Month", "")
            End If
            'ddlPriority5_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception
            lstatus.Text = "cxMonth Checked Changed Error : " + ex.ToString
        End Try
    End Sub

    Public Sub ChangePriority(ByVal Origin As String, ByVal CurrValue As String, ByVal OldValue As String)
        Try
            If Origin <> "Company" And cxCompany.Checked Then
                If CurrValue <> "0" Then
                    'If CInt(CurrValue) = CInt(ddlPriority1.SelectedValue) Then
                    '    ddlPriority1.SelectedValue = OldValue
                    'End If
                    If CInt(CurrValue) > CInt(OldValue) And CInt(OldValue) < CInt(ddlPriority1.SelectedValue) Then
                        If CInt(ddlPriority1.SelectedValue) <= CInt(CurrValue) Then
                            ddlPriority1.SelectedIndex = ddlPriority1.SelectedIndex - 1
                        End If
                    ElseIf CInt(CurrValue) < CInt(OldValue) Then
                        If CInt(ddlPriority1.SelectedValue) >= CInt(CurrValue) And CInt(ddlPriority1.SelectedValue) <= CInt(OldValue) Then
                            'lstatus.Text = Origin + " " + CurrValue + " " + OldValue
                            ddlPriority1.SelectedIndex = ddlPriority1.SelectedIndex + 1
                        End If
                    End If
                Else
                    If CInt(ddlPriority1.SelectedValue) > CInt(OldValue) Then
                        ddlPriority1.SelectedIndex = ddlPriority1.SelectedIndex - 1
                    End If
                End If
            End If

            If Origin <> "Trans" And cxTransaction.Checked Then
                If CurrValue <> "0" Then
                    'If CInt(CurrValue) = CInt(ddlPriority2.SelectedValue) Then
                    '    ddlPriority2.SelectedValue = OldValue
                    'End If
                    If CInt(CurrValue) > CInt(OldValue) And CInt(OldValue) < CInt(ddlPriority2.SelectedValue) Then
                        If CInt(ddlPriority2.SelectedValue) <= CInt(CurrValue) Then
                            ddlPriority2.SelectedIndex = ddlPriority2.SelectedIndex - 1
                        End If
                    ElseIf CInt(CurrValue) < CInt(OldValue) Then
                        If CInt(ddlPriority2.SelectedValue) >= CInt(CurrValue) And CInt(ddlPriority2.SelectedValue) <= CInt(OldValue) Then
                            ddlPriority2.SelectedIndex = ddlPriority2.SelectedIndex + 1
                        End If
                    End If
                Else
                    If CInt(ddlPriority2.SelectedValue) > CInt(OldValue) Then
                        ddlPriority2.SelectedIndex = ddlPriority2.SelectedIndex - 1
                    End If
                End If
            End If
            If Origin <> "AddParam" And cxAddParam.Checked Then
                If CurrValue <> "0" Then
                    'If CInt(CurrValue) = CInt(ddlPriority3.SelectedValue) Then
                    '    ddlPriority3.SelectedValue = OldValue
                    'End If
                    If CInt(CurrValue) > CInt(OldValue) And CInt(OldValue) < CInt(ddlPriority3.SelectedValue) Then
                        If CInt(ddlPriority3.SelectedValue) <= CInt(CurrValue) Then
                            ddlPriority3.SelectedIndex = ddlPriority3.SelectedIndex - 1
                        End If
                    ElseIf CInt(CurrValue) < CInt(OldValue) Then
                        If CInt(ddlPriority3.SelectedValue) >= CInt(CurrValue) And CInt(ddlPriority3.SelectedValue) <= CInt(OldValue) Then
                            ddlPriority3.SelectedIndex = ddlPriority3.SelectedIndex + 1
                        End If
                    End If
                Else
                    If CInt(ddlPriority3.SelectedValue) > CInt(OldValue) Then
                        ddlPriority3.SelectedIndex = ddlPriority3.SelectedIndex - 1
                    End If
                End If
            End If
            If Origin <> "Year" And cxYear.Checked Then
                If CurrValue <> "0" Then
                    'If CInt(CurrValue) = CInt(ddlPriority4.SelectedValue) Then
                    '    ddlPriority4.SelectedValue = OldValue
                    'End If
                    If CInt(CurrValue) > CInt(OldValue) And CInt(OldValue) < CInt(ddlPriority4.SelectedValue) Then
                        If CInt(ddlPriority4.SelectedValue) <= CInt(CurrValue) Then
                            ddlPriority4.SelectedIndex = ddlPriority4.SelectedIndex - 1
                        End If
                    ElseIf CInt(CurrValue) < CInt(OldValue) Then
                        If CInt(ddlPriority4.SelectedValue) >= CInt(CurrValue) And CInt(ddlPriority4.SelectedValue) <= CInt(OldValue) Then
                            ddlPriority4.SelectedIndex = ddlPriority4.SelectedIndex + 1
                        End If
                    End If
                Else
                    If CInt(ddlPriority4.SelectedValue) > CInt(OldValue) Then
                        ddlPriority4.SelectedIndex = ddlPriority4.SelectedIndex - 1
                    End If
                End If
            End If
            If Origin <> "Month" And cxMonth.Checked Then
                If CurrValue <> "0" Then
                    'If CInt(CurrValue) = CInt(ddlPriority5.SelectedValue) Then
                    '    ddlPriority5.SelectedValue = OldValue
                    'End If
                    If CInt(CurrValue) > CInt(OldValue) And CInt(OldValue) < CInt(ddlPriority5.SelectedValue) Then
                        If CInt(ddlPriority5.SelectedValue) <= CInt(CurrValue) Then
                            ddlPriority5.SelectedIndex = ddlPriority5.SelectedIndex - 1
                        End If
                    ElseIf CInt(CurrValue) < CInt(OldValue) Then
                        If CInt(ddlPriority5.SelectedValue) >= CInt(CurrValue) And CInt(ddlPriority5.SelectedValue) <= CInt(OldValue) Then
                            ddlPriority5.SelectedIndex = ddlPriority5.SelectedIndex + 1
                        End If
                    End If
                Else
                    If CInt(ddlPriority5.SelectedValue) > CInt(OldValue) Then
                        ddlPriority5.SelectedIndex = ddlPriority5.SelectedIndex - 1
                    End If
                End If
            End If

            If Origin <> "Digit" Then
                If CurrValue <> "0" Then
                    'If CInt(CurrValue) = CInt(ddlPriority6.SelectedValue) Then
                    '    ddlPriority6.SelectedValue = OldValue
                    'End If
                    If CInt(CurrValue) > CInt(OldValue) And CInt(OldValue) < CInt(ddlPriority6.SelectedValue) Then
                        If CInt(ddlPriority6.SelectedValue) <= CInt(CurrValue) Then
                            ddlPriority6.SelectedIndex = ddlPriority6.SelectedIndex - 1
                        End If
                    ElseIf CInt(CurrValue) < CInt(OldValue) Then
                        If CInt(ddlPriority6.SelectedValue) >= CInt(CurrValue) And CInt(ddlPriority6.SelectedValue) <= CInt(OldValue) Then
                            ddlPriority6.SelectedIndex = ddlPriority6.SelectedIndex + 1
                        End If
                    End If
                Else
                    If CInt(ddlPriority6.SelectedValue) > CInt(OldValue) Then
                        ddlPriority6.SelectedIndex = ddlPriority6.SelectedIndex - 1
                    End If
                End If
            End If

                If cxCompany.Checked Then
                    ViewState("Priority1") = ddlPriority1.SelectedValue
                Else
                    ViewState("Priority1") = "0"
                End If
                If cxTransaction.Checked Then
                    ViewState("Priority2") = ddlPriority2.SelectedValue
                Else
                    ViewState("Priority2") = "0"
                End If
                If cxAddParam.Checked Then
                    ViewState("Priority3") = ddlPriority3.SelectedValue
                Else
                    ViewState("Priority3") = "0"
                End If
                If cxYear.Checked Then
                    ViewState("Priority4") = ddlPriority4.SelectedValue
                Else
                    ViewState("Priority4") = "0"
                End If
                If cxMonth.Checked Then
                    ViewState("Priority5") = ddlPriority5.SelectedValue
                Else
                    ViewState("Priority5") = "0"
                End If
                ViewState("Priority6") = ddlPriority6.SelectedValue

                GenerateCode()
        Catch ex As Exception
            Throw New Exception("Change Priority Error : " + ex.ToString)
        End Try
    End Sub

    Public Function cekMaxPriority() As Integer
        Dim Result As Integer
        Try
            Result = 1
            If cxCompany.Checked Then
                Result = Result + 1
            End If
            If cxTransaction.Checked Then
                Result = Result + 1
            End If
            If cxAddParam.Checked Then
                Result = Result + 1
            End If
            If cxYear.Checked Then
                Result = Result + 1
            End If
            If cxMonth.Checked Then
                Result = Result + 1
            End If
            Return Result
        Catch ex As Exception
            Throw New Exception("Cek Max Priority Error : " + ex.ToString)
        End Try
    End Function

    Public Sub GenerateCode()
        Dim Code(5), Val As String
        Dim i, Max As Integer
        Try
            For i = 0 To 5
                Code(i) = ""
            Next
            Max = cekMaxPriority()

            If cxCompany.Checked Then
                If Max = CInt(ddlPriority1.SelectedValue) Then
                    Val = tbAlias.Text
                Else
                    Val = tbAlias.Text + ddlSeparator1.SelectedValue
                End If
                Code(ddlPriority1.SelectedIndex) = Val
            End If

            If cxTransaction.Checked Then
                If Max = CInt(ddlPriority2.SelectedValue) Then
                    Val = "&" + tbTransType.Text + "&"
                Else
                    Val = "&" + tbTransType.Text + "&" + ddlSeparator2.SelectedValue
                End If
                Code(ddlPriority2.SelectedIndex) = Val
            End If

            If cxAddParam.Checked Then
                Val = ""
                For i = 0 To ddlDigitParam.SelectedIndex
                    Val = Val + "@"
                Next
                If Max <> CInt(ddlPriority3.SelectedValue) Then
                    Val = Val + ddlSeparator3.SelectedValue
                End If
                Code(ddlPriority3.SelectedIndex) = Val
            End If

            If cxYear.Checked Then
                If ddlDigitYear.SelectedValue = "2" Then
                    Val = "$$"
                Else
                    Val = "$$$$"
                End If
                If Max <> CInt(ddlPriority4.SelectedValue) Then
                    Val = Val + ddlSeparator4.SelectedValue
                End If
                Code(ddlPriority4.SelectedIndex) = Val
            End If

            If cxMonth.Checked Then
                If ddlDigitMonth.SelectedValue = "Huruf" Then
                    If cxByYear.Checked Then
                        Val = "*"
                    Else
                        Val = "!"
                    End If
                ElseIf ddlDigitMonth.SelectedValue = "Angka" Then
                    If cxByYear.Checked Then
                        Val = "**"
                    Else
                        Val = "!!"
                    End If
                Else
                    If cxByYear.Checked Then
                        Val = "***"
                    Else
                        Val = "!!!"
                    End If
                End If
                If Max <> CInt(ddlPriority5.SelectedValue) Then
                    Val = Val + ddlSeparator5.SelectedValue
                End If
                Code(ddlPriority5.SelectedIndex) = Val
            End If

            Val = ""
            For i = 0 To ddlNumber.SelectedIndex
                Val = Val + "#"
            Next
            If Max <> CInt(ddlPriority6.SelectedValue) Then
                Val = Val + ddlSeparator6.SelectedValue
            End If

            Code(ddlPriority6.SelectedIndex) = Val

            Val = ""
            For i = 0 To 5
                Val = Val + Code(i)
            Next
            lbNewFormat.Text = Val
            lbExample.Text = GenerateSample(Val)
        Catch ex As Exception
            Throw New Exception("Generate Code Error : " + ex.ToString)
        End Try
    End Sub

    Protected Function GenerateSample(ByVal FormatStr As String) As String
        Dim Sample, tahun, abjad, bulan, monthname As String
        tahun = Today.Year.ToString
        bulan = "0" + Today.Month.ToString
        abjad = Chr(Today.Month + 64).ToString.ToUpper
        If bulan.Length = 3 Then
            bulan = bulan.Substring(1, 2)
        End If
        monthname = Today.ToString("MMM").ToUpper.Substring(0, 3)
        Sample = FormatStr
        If Sample.Trim = "" Then
            Return ""
        Else
            Sample = Sample.Replace("&", "")
            Sample = Sample.Replace("++", "PKJ")
            Sample = Sample.Replace("$$$$", tahun)
            Sample = Sample.Replace("$$", tahun.Substring(2, 2))
            Sample = Sample.Replace("!!!", monthname)
            Sample = Sample.Replace("!!", bulan)
            Sample = Sample.Replace("!", abjad)
            Sample = Sample.Replace("***", monthname)
            Sample = Sample.Replace("**", bulan)
            Sample = Sample.Replace("*", abjad)
            Sample = Sample.Replace("#########", "999999999")
            Sample = Sample.Replace("########", "99999999")
            Sample = Sample.Replace("#######", "9999999")
            Sample = Sample.Replace("######", "999999")
            Sample = Sample.Replace("#####", "99999")
            Sample = Sample.Replace("####", "9999")
            Sample = Sample.Replace("###", "999")
            Sample = Sample.Replace("##", "99")
            Sample = Sample.Replace("#", "9")
            Return Sample
        End If
    End Function

    Protected Sub ddlPriority1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriority1.SelectedIndexChanged
        Dim MaxPriority As Integer
        Try
            MaxPriority = cekMaxPriority()
            If MaxPriority < CInt(ddlPriority1.SelectedValue) Then
                ddlPriority1.SelectedValue = ViewState("Priority1")
            Else
                ChangePriority("Company", ddlPriority1.SelectedValue.ToString, ViewState("Priority1"))
            End If
        Catch ex As Exception
            lstatus.Text = "ddl Priority 1 Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ddlPriority2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriority2.SelectedIndexChanged
        Dim MaxPriority As Integer
        Try
            MaxPriority = cekMaxPriority()
            If MaxPriority < CInt(ddlPriority2.SelectedValue) Then
                ddlPriority2.SelectedValue = ViewState("Priority2")
            Else
                ChangePriority("Trans", ddlPriority2.SelectedValue.ToString, ViewState("Priority2"))
            End If
        Catch ex As Exception
            lstatus.Text = "ddl Priority 2 Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPriority3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriority3.SelectedIndexChanged
        Dim MaxPriority As Integer
        Try
            MaxPriority = cekMaxPriority()
            If MaxPriority < CInt(ddlPriority3.SelectedValue) Then
                ddlPriority3.SelectedValue = ViewState("Priority3")
            Else
                ChangePriority("AddParam", ddlPriority3.SelectedValue.ToString, ViewState("Priority3"))
            End If
        Catch ex As Exception
            lstatus.Text = "ddl Priority 3 Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ddlPriority4_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriority4.SelectedIndexChanged
        Dim MaxPriority As Integer
        Try
            MaxPriority = cekMaxPriority()
            If MaxPriority < CInt(ddlPriority4.SelectedValue) Then
                ddlPriority4.SelectedValue = ViewState("Priority4")
            Else
                ChangePriority("Year", ddlPriority4.SelectedValue.ToString, ViewState("Priority4"))
            End If
        Catch ex As Exception
            lstatus.Text = "ddl Priority 4 Changed Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub ddlPriority5_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriority5.SelectedIndexChanged
        Dim MaxPriority As Integer
        Try
            MaxPriority = cekMaxPriority()
            If MaxPriority < CInt(ddlPriority5.SelectedValue) Then
                ddlPriority5.SelectedValue = ViewState("Priority5")
            Else
                ChangePriority("Month", ddlPriority5.SelectedValue.ToString, ViewState("Priority5"))
            End If
        Catch ex As Exception
            lstatus.Text = "ddl Priority 5 Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPriority6_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPriority6.SelectedIndexChanged
        Dim MaxPriority As Integer
        Try
            MaxPriority = cekMaxPriority()
            If MaxPriority < CInt(ddlPriority6.SelectedValue) Then
                ddlPriority6.SelectedValue = ViewState("Priority6")
            Else
                ChangePriority("Digit", ddlPriority6.SelectedValue.ToString, ViewState("Priority6"))
            End If
        Catch ex As Exception
            lstatus.Text = "ddl Priority 6 Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlSeparator1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSeparator1.SelectedIndexChanged, ddlSeparator2.SelectedIndexChanged, ddlSeparator3.SelectedIndexChanged, ddlSeparator4.SelectedIndexChanged, ddlSeparator5.SelectedIndexChanged, ddlSeparator6.SelectedIndexChanged, tbTransType.TextChanged, ddlDigitParam.SelectedIndexChanged, ddlDigitYear.SelectedIndexChanged, ddlDigitMonth.SelectedIndexChanged, cxByYear.CheckedChanged, ddlNumber.SelectedIndexChanged
        Try
            GenerateCode()
        Catch ex As Exception
            lstatus.Text = "ddl Separator 1 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles DataGrid.BeforeColumnSortingGrouping
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid.PageIndexChanged
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles DataGrid.ProcessColumnAutoFilter
        bindDataGrid()
    End Sub

    Protected Sub DataGrid_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs) Handles DataGrid.CustomCallback
        Dim SelectValue As Object
        Dim val(2) As String
        Try
            val(0) = "SetDescription"
            val(1) = "SetValue"
            val(2) = "SetCode"
            SelectValue = DataGrid.GetRowValues(Convert.ToInt32(e.Parameters), val)

            lbCurrentFormat.Text = SelectValue(1).ToString
            lbNewFormat.Text = SelectValue(1).ToString
            tbDescription.Text = SelectValue(0).ToString
            lbExample.Text = GenerateSample(lbNewFormat.Text)
            ViewState("SetCode") = SelectValue(2).ToString

            ResetField()
            Disintegrator(SelectValue(1).ToString)
            PnlMain.Visible = False
            pnlInput.Visible = True
        Catch ex As Exception
            lstatus.Text = "Data Grid Custom Call Back Error : " + ex.ToString
        End Try
    End Sub
End Class
