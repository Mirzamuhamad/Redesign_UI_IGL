Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Partial Class FormComplete
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da As New SqlDataAdapter

    Protected Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
    End Sub
    Private Sub SetInit()
        Try
            'FillCombo(ddlLeaveType, "EXEC S_FindLeaves " + QuotedStr(Session("LeaveType").ToString), True, "Leave_Code", "Leave_Name", ViewState("DBConnection"))
            lbTitle.Text = Session("LbTitle")
            tbEmpNumb.Text = Session("EmpNo")
            tbEmpName.Text = Session("EmpName")
            Label1.Text = Session("Nmbr")
            BindToText(tbLeaveType, Session("LeaveType").ToString)
            BindToDate(tbHireDate, Session("TransDate").ToString)
            BindToDate(tbStartDate, Session("StartDate").ToString)
            BindToDate(tbEndDate, Session("EndDate").ToString)
            BindToText(tbTotal, Session("ActQtyTotal").ToString)
            BindToText(tbTaken, Session("ActQtyTaken").ToString)
            BindToText(tbHoliday, Session("ActQtyHoliday").ToString)
            BindToText(tbDispensasi, Session("ActQtyDispensasi").ToString)
            BindToText(tbStartTime, Session("StartTime").ToString)
            BindToText(tbEndTime, Session("EndTime").ToString)
            BindToDropList(ddlFgLess1Day, Session("FgLess1Day").ToString)
            'BindToText(tbReason, Session("ReasonLeave").ToString)
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        If Not Me.IsPostBack Then
            InitProperty()
            SetInit()
            LoadGetDt2()
            GetInfo(tbEmpNumb.Text)
        End If
    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click, btnBack2.Click
        If Session("FgComplete") = "Taken" Then

            If Session("FgType") = "Leave" Then
                Response.Redirect("..\..\Transaction\TrLeaveOut\TrLeaveOut.aspx?KeyId=" + Session("KeyId") + "&ContainerId=LeaveOutId")
            Else
                Response.Redirect("..\..\Transaction\TrLeaveOut\TrLeaveOut.aspx?KeyId=" + Session("KeyId") + "&ContainerId=LeavePermitId")
            End If
        End If
    End Sub

    Protected Sub btnOK2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK2.Click
        Dim SQLString, result As String
        Dim Tahun, Bulan As String
        'Dim PrimaryKey() As String
        Try
            'PrimaryKey = Session("Nmbr").ToString.Trim.Split("|")

            If (tbStartTime.Text.Trim <> "00:00") And (tbEndTime.Text.Trim <> "00:00") And (tbStartTime.Text.Trim <> "") And (tbEndTime.Text.Trim <> "") Then
                If tbStartTime.Text.Trim > tbEndTime.Text.Trim Then
                    lbstatus.Text = MessageDlg("Start Hour can't greater than End Hour")
                    tbStartTime.Focus()
                    Exit Sub
                End If
            End If
            Tahun = CInt(ViewState("GLYear"))
            Bulan = CInt(ViewState("GLPeriod"))
            If Session("FgComplete") = "Taken" Then
                SQLString = "Declare @A VarChar(255) EXEC S_PELEaveOutComplete '" + Session("Nmbr").ToString + _
                "', '" + Session("EmpNo") + "'," + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + _
                ", " + QuotedStr(tbStartTime.Text) + ", " + QuotedStr(Format(tbEndDate.SelectedValue, "yyyy-MM-dd")) + _
                ", " + QuotedStr(tbEndTime.Text) + ", 0, " + tbTaken.Text + ", " + tbHoliday.Text + _
                ", " + tbDispensasi.Text + ", " + tbTotal.Text + ", '" + tbReason.Text + _
                "', " + Tahun + ", " + Bulan + ", " + _
                QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"

                result = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                If result.Length > 2 Then
                    lbstatus.Text = result
                Else
                    If Session("FgType") = "Leave" Then
                        Response.Redirect("..\..\Transaction\TrLeaveOut\TrLeaveOut.aspx?KeyId=" + Session("KeyId") + "&ContainerId=LeaveOutId")
                    Else
                        Response.Redirect("..\..\Transaction\TrLeaveOut\TrLeaveOut.aspx?KeyId=" + Session("KeyId") + "&ContainerId=LeavePermitId")
                    End If

                    'Response.Redirect("..\..\Transaction\TrLeaveOut\TrLeaveOut.aspx?KeyId=" + Session("KeyId") + "&ContainerId=TrLeaveOutID")
                End If
            End If
        Catch ex As Exception
            lbstatus.Text = "btnOK2_Click Error : " + ex.ToString
        End Try

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
    Private Function GetStringDt(ByVal Nmbr As String, ByVal EmpNumb As String, ByVal ActStartDate As DateTime, ByVal ActStartTime As String, ByVal ActEndDate As DateTime, ByVal ActEndTime As String) As String
        Return "EXEC S_PELeaveOutGetDt2 " + QuotedStr(Nmbr) + ", " + QuotedStr(EmpNumb) + ", " + QuotedStr(ActStartDate) + ", " + QuotedStr(ActStartTime) + ", " + QuotedStr(ActEndDate) + ", " + QuotedStr(ActEndTime)
    End Function
    'Private Sub LoadGetDt2()
    '    Dim Dr As DataRow
    '    Dim Ds As DataSet
    '    Try

    '        Ds = SQLExecuteQuery("EXEC S_PELeaveOutGetDt2 " + QuotedStr(Label1.Text) + ", " + QuotedStr(tbEmpNumb.Text) + ", '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbStartTime.Text) + ", '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbEndTime.Text), ViewState("DBConnection").ToString)
    '        Ds = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

    '        GridDt.DataSource = Ds.Tables(0)
    '        GridDt.DataBind()
    '    Catch ex As Exception
    '        Throw New Exception("LoadAbsInfo Error " + ex.ToString)
    '    End Try
    'End Sub

    Private Sub LoadGetDt2()
        Dim SqlString, sqlstring1 As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_PELeaveOutGetDt2 " + QuotedStr(Label1.Text) + ", " + QuotedStr(tbEmpNumb.Text) + ", '" + Format(tbStartDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbStartTime.Text) + ", '" + Format(tbEndDate.SelectedValue, "yyyy-MM-dd") + "', " + QuotedStr(tbEndTime.Text)
            SQLExecuteNonQuery(SqlString, ViewState("DBConnection").ToString)

            SqlString1 = "Select LeaveDate, FgLeaves, Remark from PELeaveOutDt2 Where TransNmbr = " + QuotedStr(Label1.Text) + " And EmpNumb = " + QuotedStr(tbEmpNumb.Text)
            DS = SQLExecuteQuery(sqlstring1, ViewState("DBConnection"))

            GridDt.DataSource = DS.Tables(0)
            GridDt.DataBind()
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub

    Private Sub GetInfo(ByVal Product As String)
        Dim SqlString As String
        Dim DS As DataSet
        Try
            SqlString = "EXEC S_PELeaveOutInfo " + QuotedStr(tbEmpNumb.Text) + _
            ", " + QuotedStr(Format(tbStartDate.SelectedValue, "yyyy-MM-dd")) + ", " + tbTaken.Text

            DS = SQLExecuteQuery(SqlString, ViewState("DBConnection"))

            GridInfo.DataSource = DS.Tables(0)
            GridInfo.DataBind()
            PnlInfo.Visible = True
            lbInfo.Visible = DS.Tables(0).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("get info error : " + ex.ToString)
        End Try
    End Sub
End Class
