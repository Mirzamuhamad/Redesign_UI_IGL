Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Partial Class FormComplete
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * FROM V_PEOvertimeDt"

    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
    End Sub
    Private Sub SetInit()
        Try
            lblOvertime.Text = Session("Nmbr")
            tbDate.SelectedDate = Session("TransDate")
            tbEmpNo.Text = Session("EmpNo")

            tbActMinuteBreak.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            InitProperty()
            SetInit()
            FillTextBoxHd(lblOvertime.Text, tbEmpNo.Text)
        End If
    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click, btnBack2.Click
        If Session("FgComplete") = "Overtime" Then
            ViewState("StateHd") = "View"
            Response.Redirect("..\..\Transaction\TrEmpOvertime\TrEmpOvertime.aspx?KeyId=" + Session("KeyId") + "&ContainerId=TrEmpOverTimeId")
        End If
    End Sub

    Protected Sub btnOK2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK2.Click
        Dim SQLString, result As String
        Dim PrimaryKey() As String
        PrimaryKey = Session("Nmbr").ToString.Trim.Split("|")

        If Session("FgComplete") = "Overtime" Then
            Dim GLYear, GLPeriod As Integer
            GLYear = ViewState("GLYear")
            GLPeriod = ViewState("GLPeriod")
            SQLString = "DECLARE @A VarChar(255) EXEC S_PEOvertimeComplete " + QuotedStr(lblOvertime.Text) + ", " + QuotedStr(tbEmpNo.Text) + ", " + QuotedStr(Format(tbActStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbActStartTime.Text) + ", " + QuotedStr(Format(tbActEndDate.SelectedValue, "yyyy-MM-dd")) + ", " + QuotedStr(tbActEndTime.Text) + ", " + tbActMinuteBruto.Text.Replace(",", "") + ", " + tbActMinuteBreak.Text + ", " + tbActMinuteNetto.Text.Replace(",", "") + ", " + tbActHournetto.Text + ", " + QuotedStr(ddlActFgMealAllowance.SelectedValue) + ", " + tbOTHour.Text + ", " + GLYear.ToString + ", " + GLPeriod.ToString + ", " + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A "
            result = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)

            'Dim Dr As DataRow
            Dim Dt As DataTable
            Dim SQL As String

            SQL = "SELECT A.TransNmbr FROM PEOvertimeHd A INNER JOIN PEOvertimeDt B ON A.TransNmbr = B.TransNmbr WHERE A.Status = 'P' AND B.DoneComplete = 'N' AND A.TransNmbr = " + QuotedStr(Session("Nmbr"))
            Dt = SQLExecuteQuery(SQL, ViewState("DBConnection").ToString).Tables(0)

            If Dt.Rows.Count <> 0 Then
            Else
                Dim SQL2 As String
                SQL2 = " UPDATE PEOvertimeHd SET Status = 'F' WHERE TransNmbr = " + QuotedStr(Session("Nmbr"))
                SQLExecuteNonQuery(SQL2, ViewState("DBConnection").ToString)
            End If

            If result.Length > 2 Then
                lbstatus.Text = result
            Else
                ViewState("StateHd") = "View"
                Response.Redirect("..\..\Transaction\TrEmpOverTime\TrEmpOverTime.aspx?KeyId=" + Session("KeyId") + "&ContainerId=TrEmpOverTimeID")

            End If
        End If
    End Sub

    Protected Sub InitProperty()
        ViewState("DBConnection") = Session(Session("KeyId"))("DBConnection")
        ViewState("UserId") = Session(Session("KeyId"))("UserId")
        ViewState("UserName") = Session(Session("KeyId"))("UserName")
        ViewState("FgAdmin") = Session(Session("KeyId"))("FgAdmin")
        ViewState("Currency") = Session(Session("KeyId"))("Currency")
        ViewState("GLYear") = Session(Session("KeyId"))("Year")
        ViewState("GLPeriod") = Session(Session("KeyId"))("Period")
        ViewState("GLPeriodName") = Session(Session("KeyId"))("PeriodName")
        ViewState("CompanyName") = Session(Session("KeyId"))("CompanyName")
        ViewState("Address1") = Session(Session("KeyId"))("Address1")
        ViewState("Address2") = Session(Session("KeyId"))("Address2")
        ViewState("PageSizeGrid") = Session(Session("KeyId"))("PageSizeGrid")
        ViewState("1Payment") = Session(Session("KeyId"))("1Payment")
        ViewState("DigitRate") = Session(Session("KeyId"))("DigitRate")
        ViewState("DigitQty") = Session(Session("KeyId"))("DigitQty")
        ViewState("DigitHome") = Session(Session("KeyId"))("DigitHome")
        ViewState("DigitPercent") = Session(Session("KeyId"))("DigitPercent")
        ViewState("ServerDate") = Session(Session("KeyId"))("ServerDate")
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String, ByVal EmpNo As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "TransNmbr = " + QuotedStr(Nmbr) + " AND EmpNumb = " + QuotedStr(EmpNo), ViewState("DBConnection").ToString)
            BindToDate(tbStartDate, Dt.Rows(0)("StartDate").ToString)
            BindToDate(tbEndDate, Dt.Rows(0)("EndDate").ToString)
            BindToText(tbStartTime, Dt.Rows(0)("StartTime").ToString)
            BindToText(tbEndTime, Dt.Rows(0)("EndTime").ToString)
            BindToText(tbMinuteBruto, Dt.Rows(0)("MinuteBruto").ToString)
            BindToText(tbMinuteBreak, Dt.Rows(0)("MinuteBreak").ToString)
            BindToText(tbMinuteNetto, Dt.Rows(0)("MinuteNetto").ToString)
            BindToText(tbEmpNo, Dt.Rows(0)("EmpNumb").ToString)
            BindToText(tbEmpName, Dt.Rows(0)("EmpName").ToString)
            BindToDropList(ddlDayType, Dt.Rows(0)("DayType").ToString)
            BindToDate(tbActStartDate, Dt.Rows(0)("ActStartDate").ToString)
            BindToDate(tbActEndDate, Dt.Rows(0)("ActEndDate").ToString)
            BindToText(tbActStartTime, Dt.Rows(0)("ActStartHour").ToString)
            BindToText(tbActEndTime, Dt.Rows(0)("ActEndHour").ToString)
            BindToText(tbActMinuteBruto, Dt.Rows(0)("ActMinuteBruto").ToString)
            BindToText(tbActMinuteBreak, Dt.Rows(0)("ActMinuteBreak").ToString)
            BindToText(tbActMinuteNetto, Dt.Rows(0)("ActMinuteNetto").ToString)
            BindToText(tbActHournetto, FormatNumber(Dt.Rows(0)("ActHourNetto").ToString, 2))
            BindToDropList(ddlActFgMealAllowance, Dt.Rows(0)("ActFgMealAllowance").ToString)
            BindToText(tbOTHour, FormatNumber(Dt.Rows(0)("OTHour").ToString, 2))

            Dim Dr As DataRow
            Dim Ds As DataSet

            Ds = SQLExecuteQuery("EXEC S_PEOverTimeInfoAbs " + QuotedStr(EmpNo) + ", '" + Format(tbActStartDate.SelectedValue, "yyyy-MM-dd") + "' ", ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                If tbActStartTime.Text < TrimStr(Dr("AbsIn").ToString) Then
                    tbActStartTime.Text = TrimStr(Dr("AbsIn").ToString)
                End If
                If tbActEndDate.SelectedDate > Dr("AbsDateOut").ToString Then
                    tbActEndDate.SelectedDate = Dr("AbsDateOut").ToString
                    tbActEndTime.Text = TrimStr(Dr("AbsOut").ToString)
                ElseIf tbActEndDate.SelectedDate = Dr("AbsDateOut").ToString Then
                    If tbActEndTime.Text > TrimStr(Dr("AbsOut").ToString) Then
                        tbActEndTime.Text = TrimStr(Dr("AbsOut").ToString)
                    End If
                End If
            End If

            'CekActTime()
            LoadAbsInfo()
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Private Sub CekActTime()
        Dim StartH, StartM, EndH, EndM, iStart, iEnd, iTot, iDay As Double
        Dim Sdd, Smm, Syy, Edd, Emm, Eyy As Integer
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            If tbActEndTime.Text.Trim <> "" Then
                'StartH = CFloat(Strings.Mid(tbActStartTime.Text, 1, 2))
                'StartM = CFloat(Strings.Mid(tbActStartTime.Text, 4, 2))
                'EndH = CFloat(Strings.Mid(tbActEndTime.Text, 1, 2))
                'EndM = CFloat(Strings.Mid(tbActEndTime.Text, 4, 2))
                'Syy = tbActStartDate.SelectedDate.Year
                'Smm = tbActStartDate.SelectedDate.Month
                'Sdd = tbActStartDate.SelectedDate.Day
                'Eyy = tbActEndDate.SelectedDate.Year
                'Emm = tbActEndDate.SelectedDate.Month
                'Edd = tbActEndDate.SelectedDate.Day
                If (tbActStartDate.SelectedDate = tbStartDate.SelectedDate) And (tbStartTime.Text > tbActStartTime.Text) Then
                    lbstatus.Text = MessageDlg("Your Act. Start Hour is earlier than your Start Time")
                    tbActStartTime.Text = tbStartTime.Text
                    StartH = CFloat(Strings.Mid(tbActStartTime.Text, 1, 2))
                    StartM = CFloat(Strings.Mid(tbActStartTime.Text, 4, 2))
                End If
                If (tbActStartDate.SelectedDate = tbActEndDate.SelectedDate) And ((EndH < StartH) Or ((StartH = EndH) And (EndM < StartM))) Then
                    lbstatus.Text = MessageDlg("Your Act. End Hour is earlier than your Start Time")
                    tbActEndTime.Text = "00:00"
                    tbActMinuteBruto.Text = "0"
                    tbActEndTime.Focus()
                    Exit Sub
                End If
                'iDay = (CFloat(tbActEndDate.Text) - CFloat(tbActStartDate.Text)) * 24 * 60
                'iStart = (StartH * 60) + StartM
                'iEnd = (EndH * 60) + EndM
                'iTot = iEnd - iStart + iDay
                'tbActMinuteBruto.Text = iTot

                Ds = SQLExecuteQuery("EXEC S_PEOvertimeCekTime " + QuotedStr(tbActStartDate.SelectedDate) + ", " + QuotedStr(tbActEndDate.SelectedDate) + ", " + QuotedStr(tbActStartTime.Text) + ", " + QuotedStr(tbActEndTime.Text), ViewState("DBConnection").ToString)
                If Ds.Tables(0).Rows.Count <> 0 Then
                    Dr = Ds.Tables(0).Rows(0)
                    If TrimStr(Dr("Pesan").ToString) <> "" Then
                        lbstatus.Text = MessageDlg(TrimStr(Dr("Pesan").ToString))
                        tbActEndTime.Text = "00:00"
                        tbActMinuteBruto.Text = 0
                        Exit Sub
                    Else
                        tbActMinuteBruto.Text = TrimStr(FormatNumber(Dr("ITot"), 0))
                    End If
                Else
                    tbActMinuteBruto.Text = 0
                End If
            End If
        Catch ex As Exception
            Throw New Exception("CekTime Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActStartDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActStartDate.SelectionChanged
        Try
            If tbStartDate.SelectedDate > tbActStartDate.SelectedDate Then
                lbstatus.Text = MessageDlg("Your Act. Start Date is earlier than your Start Date")
                tbActStartDate.SelectedDate = tbStartDate.SelectedDate
            End If
            If (Not tbActEndDate.IsNull) Then
                If tbActEndDate.SelectedDate < tbActStartDate.SelectedDate Then
                    tbActEndDate.SelectedDate = tbActStartDate.SelectedDate
                End If
                CekActTime()
            End If
            LoadAbsInfo()
            tbActMinuteNetto.Text = FormatNumber(CFloat(tbActMinuteBruto.Text) - CFloat(tbActMinuteBreak.Text), 0)

            LoadHour()
        Catch ex As Exception
            Throw New Exception("tbActStartDate_SelectionChanged Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActEndDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActEndDate.SelectionChanged
        Try
            If tbActEndDate.SelectedDate < tbActStartDate.SelectedDate Then
                lbstatus.Text = MessageDlg("Your Act. End Date is earlier than your Act. Start Date")
                tbActEndDate.SelectedDate = tbActStartDate.SelectedDate
            End If
            CekActTime()
            tbActMinuteNetto.Text = FormatNumber(CFloat(tbActMinuteBruto.Text) - CFloat(tbActMinuteBreak.Text), 0)

            LoadHour()
        Catch ex As Exception
            Throw New Exception("tbActEndDate_SelectionChanged Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActStartTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActStartTime.TextChanged
        Dim StartH, StartM As Double
        Try
            If tbActStartTime.Text.Trim <> "" Then
                StartH = CFloat(Strings.Mid(tbActStartTime.Text, 1, 2))
                StartM = CFloat(Strings.Mid(tbActStartTime.Text, 4, 2))
                If (StartH > 23) Or (StartM > 59) Then
                    lbstatus.Text = MessageDlg("Your Act. Start Hour is invalid")
                    tbActStartTime.Text = "00:00"
                    tbActStartTime.Focus()
                    Exit Sub
                End If
                CekActTime()
            End If
            tbActMinuteNetto.Text = FormatNumber(CFloat(tbActMinuteBruto.Text) - CFloat(tbActMinuteBreak.Text), 0)
            LoadHour()
        Catch ex As Exception
            Throw New Exception("tbActStartTime_TextChanged Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActEndTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActEndTime.TextChanged
        Dim EndH, EndM As Double
        Try
            If tbActEndTime.Text.Trim <> "" Then
                EndH = CFloat(Strings.Mid(tbActEndTime.Text, 1, 2))
                EndM = CFloat(Strings.Mid(tbActEndTime.Text, 4, 2))
                If (EndH > 23) Or (EndM > 59) Then
                    lbstatus.Text = MessageDlg("Your Act. End Time is invalid")
                    tbActEndTime.Text = "00:00"
                    tbActEndTime.Focus()
                    Exit Sub
                End If
                CekActTime()
            End If
            tbActMinuteNetto.Text = FormatNumber(CFloat(tbActMinuteBruto.Text) - CFloat(tbActMinuteBreak.Text), 0)
            LoadHour()
        Catch ex As Exception
            Throw New Exception("tbActEndTime_TextChanged Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub tbActMinuteBreak_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbActMinuteBreak.TextChanged
        Try
            If tbActMinuteBreak.Text.Trim = "" Then
                tbActMinuteBreak.Text = "0"
            End If
            tbActMinuteNetto.Text = FormatNumber(CFloat(tbActMinuteBruto.Text) - CFloat(tbActMinuteBreak.Text), 0)

            LoadHour()
            
        Catch ex As Exception
            Throw New Exception("tbActMinuteBreak_TextChanged Error " + ex.ToString)
        End Try
    End Sub

    Private Sub LoadAbsInfo()
        Dim Dr As DataRow
        Dim Ds As DataSet
        Try
            Ds = SQLExecuteQuery("EXEC S_PEOverTimeInfoAbs " + QuotedStr(tbEmpNo.Text) + ", '" + Format(tbActStartDate.SelectedValue, "yyyy-MM-dd") + "' ", ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbTimeIn.Text = TrimStr(Dr("AbsIn").ToString)
                TbTimeOut.Text = TrimStr(Dr("AbsOut").ToString)
            End If
        Catch ex As Exception
            Throw New Exception("LoadAbsInfo Error " + ex.ToString)
        End Try
    End Sub

    Private Sub LoadHour()
        Dim Dr, Dw As DataRow
        Dim Ds, Dt As DataSet
        Try
            Ds = SQLExecuteQuery("EXEC S_PEOvertimeGetHourNetto " + tbActMinuteNetto.Text.Replace(",", ""), ViewState("DBConnection").ToString)
            If Ds.Tables(0).Rows.Count = 1 Then
                Dr = Ds.Tables(0).Rows(0)
                tbActHournetto.Text = TrimStr(FormatNumber(Dr("HourNetto").ToString, 2))
            End If

            'netto change
            Dt = SQLExecuteQuery("EXEC S_PEOvertimeGetHourOT " + QuotedStr(ddlDayType.SelectedValue) + ", " + tbActHournetto.Text, ViewState("DBConnection").ToString)
            If Dt.Tables(0).Rows.Count = 1 Then
                Dw = Dt.Tables(0).Rows(0)
                tbOTHour.Text = TrimStr(FormatNumber(Dw("HourOT").ToString, 2))
            End If

        Catch ex As Exception
            Throw New Exception("LoadAbsInfo Error " + ex.ToString)
        End Try
    End Sub

 
End Class
