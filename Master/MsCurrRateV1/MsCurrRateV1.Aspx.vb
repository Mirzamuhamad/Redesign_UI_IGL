Imports System.Data
Imports System.Data.SqlClient
Partial Class Master_MsCurrRateV1_MsCurrRateV1
    Inherits System.Web.UI.Page
    Dim Delete As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                Delete = False
                FillCombo(ddlPeriode, "EXEC S_MsCurrRateGetHistory", False, "CurrDateStr", "Periode", ViewState("DBConnection"))
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                'tbStartDate.SelectedDate = ViewState("ServerDate") 'Date.Today
                'tbEndDate.SelectedDate = DateAdd(DateInterval.Day, 6, tbStartDate.SelectedDate)
                FillDate()
                pnlView.Visible = False
                BindData()
            End If
            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnView" Then
                    BindData()
                End If

                If Not ViewState("Sender") Is Nothing Then
                    ViewState("Sender") = Nothing
                End If
                If Not Session("Result") Is Nothing Then
                    Session("Result") = Nothing
                End If
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
        Dim SqlString As String
        Dim Dt, Dt2 As DataTable
        Dim Dr, Dr2 As DataRow
        Dim LastDate As DateTime

        Try
            tempDS = SQLExecuteQuery("EXEC S_MsCurrRateGetDtV1 " + QuotedStr(Format(tbStartDate.SelectedDate, "yyyyMMdd")) + ", " + QuotedStr(Format(tbEndDate.SelectedDate, "yyyyMMdd")), ViewState("DBConnection").ToString)
            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()

            SqlString = "EXEC S_MsCurrRateGetLast"

            Dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                LastDate = Dr("MaxDate").ToString
            End If

            If tbEndDate.SelectedDate = LastDate Then
                btnDelete.Visible = True
            Else
                btnDelete.Visible = False
            End If

            If Delete = True Then
                Dt2 = SQLExecuteQuery("SELECT * FROM MsCurrRate WHERE  EndDate = " + QuotedStr(Format(LastDate, "yyyyMMdd")), ViewState("DBConnection").ToString).Tables(0)
                If Dt2.Rows.Count > 0 Then
                    Dr2 = Dt2.Rows(0)
                    BindToDate(tbStartDate, Dr2("CurrDate").ToString)
                    BindToDate(tbEndDate, Dr2("EndDate").ToString)
                End If
            End If

            CekData(tbStartDate.SelectedDate, tbEndDate.SelectedDate)
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Dim SQLString As String
        Dim lbAdd, lbCode As Label
        Dim tbRate, tbTaxRate As TextBox
        Dim GVR As GridViewRow
        Dim DDLOperator As DropDownList
        Dim i As Integer
        Dim exe As Boolean
        Try
            If pnlView.Visible = True Then
                pnlView.Visible = False
            End If

            exe = True

            For i = 0 To DataGrid.Rows.Count - 1
                GVR = DataGrid.Rows(i)
                lbCode = GVR.FindControl("CurrCode")
                tbRate = GVR.FindControl("CurrRate")
                tbTaxRate = GVR.FindControl("TaxRate")
                tbRate.Text = tbRate.Text.Replace(",", "")
                tbTaxRate.Text = tbTaxRate.Text.Replace(",", "")

                If tbRate.Text.Trim = "" Then
                    tbRate.Text = "0"
                End If
                If tbTaxRate.Text.Trim = "" Then
                    tbTaxRate.Text = "0"
                End If

                If Not IsNumeric(tbRate.Text) Then
                    lbstatus.Text = "Rate for " + lbCode.Text + " must in numeric format"
                    exe = False
                    tbRate.Focus()
                    Exit For
                End If
                If Not IsNumeric(tbTaxRate.Text) Then
                    lbstatus.Text = "Rate for " + lbCode.Text + " must in numeric format"
                    exe = False
                    tbTaxRate.Focus()
                    Exit For
                End If
            Next

            If exe Then
                For i = 0 To DataGrid.Rows.Count - 1
                    GVR = DataGrid.Rows(i)

                    lbCode = GVR.FindControl("CurrCode")
                    lbAdd = GVR.FindControl("FgAdd")
                    tbRate = GVR.FindControl("CurrRate")
                    tbTaxRate = GVR.FindControl("TaxRate")
                    tbRate.Text = tbRate.Text.Replace(",", "")
                    tbTaxRate.Text = tbTaxRate.Text.Replace(",", "")
                    DDLOperator = GVR.FindControl("Operator")


                    If lbAdd.Text = "Y" Then
                        SQLString = "INSERT INTO MsCurrRate (CurrDate, CurrCode, CurrRate, TaxRate, Operator, EndDate) " + _
                        " SELECT " + QuotedStr(Format(tbStartDate.SelectedDate, "yyyy-MM-dd")) + ", " + _
                        QuotedStr(lbCode.Text) + ", " + tbRate.Text + ", " + tbTaxRate.Text + ", " + QuotedStr(DDLOperator.SelectedValue) + ", " + _
                        QuotedStr(Format(tbEndDate.SelectedDate, "yyyy-MM-dd"))
                    Else
                        SQLString = "UPDATE MsCurrRate SET CurrRate = " + tbRate.Text + ", TaxRate = " + tbTaxRate.Text + _
                        ", Operator= " + QuotedStr(DDLOperator.SelectedValue) + _
                        ", EndDate = " + QuotedStr(Format(tbEndDate.SelectedDate, "yyyy-MM-dd")) + _
                        " WHERE CurrDate = " + QuotedStr(Format(tbStartDate.SelectedDate, "yyyy-MM-dd")) + _
                        " AND CurrCode = " + QuotedStr(lbCode.Text)
                    End If
                    SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                Next
                BindData()
            End If

            tbEndDate.Enabled = False
            FillCombo(ddlPeriode, "EXEC S_MsCurrRateGetHistory", False, "CurrDateStr", "Periode", ViewState("DBConnection"))
        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbEndDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEndDate.SelectionChanged
        Try
            If pnlView.Visible = True Then
                pnlView.Visible = False
            End If

            If tbEndDate.SelectedDate < tbStartDate.SelectedDate Then
                lbstatus.Text = MessageDlg("End Date must greater than Start Date!")
                tbEndDate.SelectedDate = DateAdd(DateInterval.Day, 6, tbStartDate.SelectedDate)
                Exit Sub
            End If
            BindData()
        Catch ex As Exception
            lbstatus.Text = "Date Selection Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CurrRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim tb As TextBox
        Dim Count As Integer
        'Dim dgi As GridViewRow

        Try
            tb = sender
            If tb.ID = "CurrRate" Then
                Count = DataGrid.EditIndex
                'lbstatus.Text = CStr(Count)
                'Exit Sub
                'dgi = DataGrid.Rows(Count) '-1 for allowpaging = False   - 2 allowpaging = True
                'tbRate = dgi.FindControl("CurrRate")
                'tbTaxRate = dgi.FindControl("TaxRate")
                'tbTaxRate.Text = tbRate.Text
            End If
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub


    'Protected Sub CurrRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CurrRate.TextChanged
    'Dim CurrRate, tb As TextBox
    'Dim dgi As GridViewRow
    'Dim Count As Integer
    '   Try
    '      tb = sender
    '     Count = DataGrid.SelectedValue
    '    dgi = DataGrid.Rows(Count)
    '   CurrRate = dgi.FindControl("CurrRate")

    'Catch ex As Exception
    '   lbstatus.Text = ex.ToString
    'End Try
    'End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        Try
            If pnlView.Visible = False Then
                pnlView.Visible = True
            Else
                pnlView.Visible = False
            End If
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dim SqlString As String
        Dim Dt As DataTable
        Dim Dr As DataRow
        Dim LastDate As DateTime
        Try
            If pnlView.Visible = True Then
                pnlView.Visible = False
            End If

            SqlString = "EXEC S_MsCurrRateGetLast"

            Dt = SQLExecuteQuery(SqlString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                LastDate = Dr("MaxDate").ToString
            End If

            tbStartDate.SelectedDate = DateAdd(DateInterval.Day, 1, LastDate)
            tbEndDate.SelectedDate = DateAdd(DateInterval.Day, 6, tbStartDate.SelectedDate)
            BindData()
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim SqlString As String
        Try
            SqlString = "DELETE FROM MsCurrRate WHERE CurrDate = " + QuotedStr(Format(tbStartDate.SelectedDate, "yyyy-MM-dd"))
            SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)
            Delete = True
            BindData()
            FillCombo(ddlPeriode, "EXEC S_MsCurrRateGetHistory", False, "CurrDateStr", "Periode", ViewState("DBConnection"))
        Catch ex As Exception
            lbstatus.Text = ex.ToString
        End Try
    End Sub

    Protected Sub OnConfirm(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        If pnlView.Visible = True Then
            pnlView.Visible = False
        End If

        Dim confirmValue As String = Request.Form("confirm_value")

        If confirmValue = "Yes" Then
            execBtnGo(btnDelete)
            Delete = True
        ElseIf confirmValue = "No" Then
            execBtnGo(btnDelete)
            Delete = False
        End If
    End Sub

    Sub execBtnGo(ByVal sender As Object)
        Dim SqlString As String
        Try
            'SqlString2 = "EXEC S_MsCurrRateGetLast"

            'Dt = SQLExecuteQuery(SqlString2, ViewState("DBConnection").ToString).Tables(0)
            'If Dt.Rows.Count > 0 Then
            '    Dr = Dt.Rows(0)
            '    LastDate = Dr("MaxDate").ToString            
            'End If

            'If LastDate <> tbEndDate.SelectedDate Then
            '    lbstatus.Text = MessageDlg("can not delete data , you must delete the previous data!")
            '    Exit Sub
            'End If

            SqlString = "DELETE FROM MsCurrRate WHERE CurrDate = " + QuotedStr(Format(tbStartDate.SelectedDate, "yyyy-MM-dd"))
            SQLExecuteScalar(SqlString, ViewState("DBConnection").ToString)

        Catch ex As Exception
            lbstatus.Text = "Go Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Dt As DataTable
        Dim SQLString As String
        Dim Dr As DataRow
        Try
            SQLString = "SELECT * FROM MsCurrRate WHERE CONVERT(VARCHAR(12),CurrDate,112) + ' - ' + CONVERT(VARCHAR(12),EndDate,112) = " + QuotedStr(ddlPeriode.SelectedValue)

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToDate(tbStartDate, Dr("CurrDate").ToString)
                BindToDate(tbEndDate, Dr("EndDate").ToString)
            Else
                tbStartDate.Clear()
                tbEndDate.Clear()
            End If

            BindData()
            pnlView.Visible = False
        Catch ex As Exception
            lbstatus.Text = "btnOK_Click Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub CekData(ByVal StartDate As DateTime, ByVal EndDate As DateTime)
        Dim Dt As DataTable
        Dim SQLString As String
        Try
            SQLString = "SELECT * FROM MsCurrRate WHERE CurrDate = " + QuotedStr(Format(StartDate, "yyyyMMdd"))

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then                
                tbEndDate.Enabled = False
            Else
                tbEndDate.Enabled = True
            End If
        Catch ex As Exception
            lbstatus.Text = "CekData Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillDate()
        Dim Dt As DataTable
        Dim SQLString As String
        Dim Dr As DataRow
        Try
            SQLString = "SELECT TOP 1 CurrDate, EndDate FROM MsCurrRate " + _
                        " ORDER BY CurrDate DESC"

            Dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)
            If Dt.Rows.Count > 0 Then
                Dr = Dt.Rows(0)
                BindToDate(tbStartDate, Dr("CurrDate").ToString)
                BindToDate(tbEndDate, Dr("EndDate").ToString)
            Else
                tbStartDate.SelectedDate = ViewState("ServerDate")
                tbEndDate.SelectedDate = DateAdd(DateInterval.Day, 6, tbStartDate.SelectedDate)
            End If

            'tbStartDate.SelectedDate = DateAdd(DateInterval.Day, 1, tbStartDate.SelectedDate)
            'tbEndDate.SelectedDate = DateAdd(DateInterval.Day, 6, tbStartDate.SelectedDate)
        Catch ex As Exception
            lbstatus.Text = "CekData Error : " + ex.ToString
        End Try
    End Sub

End Class
