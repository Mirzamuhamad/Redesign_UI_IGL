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
            lblEmpNo.Text = Session("EmpNo") + " - " + Session("EmpName")
            tbStartHour.Text = Session("StartHour")
            tbEndHour.Text = Session("EndHour")
            tbRemark.Text = ""
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            InitProperty()
            SetInit()
        End If
    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click, btnBack2.Click
        If Session("FgComplete") = "Izin" Then
            Response.Redirect("..\..\Transaction\TrIzin\TrIzin.aspx?KeyId=" + Session("KeyId") + "&ContainerId=TrIzinID")
        End If
    End Sub

    Protected Sub btnOK2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK2.Click
        Dim SQLString, result As String
        Dim PrimaryKey() As String
        PrimaryKey = Session("Nmbr").ToString.Trim.Split("|")

        If tbRemark.Text.Trim = "" Then
            lbstatus.Text = MessageDlg("Remark must be filled")
            tbRemark.Focus()
            Exit Sub
        End If

        If (tbStartHour.Text.Trim <> "00:00") And (tbEndHour.Text.Trim <> "00:00") And (tbStartHour.Text.Trim <> "") And (tbEndHour.Text.Trim <> "") Then
            If tbStartHour.Text.Trim > tbEndHour.Text.Trim Then
                lbstatus.Text = MessageDlg("Start Hour can't greater than End Hour")
                tbStartHour.Focus()
                Exit Sub
            End If
        End If

        If Session("FgComplete") = "Izin" Then
            SQLString = "Declare @A VarChar(255) EXEC S_PEIzinComplete '" + Session("Nmbr").ToString + "', '" + Session("EmpNo") + "'," + QuotedStr(tbStartHour.Text) + ", " + QuotedStr(tbEndHour.Text) + ", '" + tbRemark.Text + "'," + QuotedStr(ViewState("UserId")) + ", @A OUT SELECT @A"
            result = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
            If result.Length > 2 Then
                lbstatus.Text = result
            Else
                Response.Redirect("..\..\Transaction\TrIzin\TrIzin.aspx?KeyId=" + Session("KeyId") + "&ContainerId=TrIzinID")
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
End Class
