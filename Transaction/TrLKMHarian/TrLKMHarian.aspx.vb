Imports System.Data
Imports System.Data.SqlClient
Imports MessagingToolkit.QRCode.Codec
Imports MessagingToolkit.QRCode.Codec.Data
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging

Partial Class Transaction_TrLKMHarian_TrLKMHarian
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                FillCombo(ddlStartWeek, "Exec S_PLLKMHarianClosingGetStartWeek", False, "WeekNo", "WeekNo", ViewState("DBConnection"))
                FillCombo(ddlEndWeek, "Exec S_PLLKMHarianClosingGetEndWeek", False, "WeekNo", "WeekNo", ViewState("DBConnection"))
                FillCombo(ddlDivision, "SELECT * FROM V_Division", False, "DivisionCode", "DivisionName", ViewState("DBConnection"))

                pnlService.Visible = True
                BindData()
            End If

            If Not Session("Result") Is Nothing Then

                Session("Result") = Nothing
                ViewState("Sender") = Nothing
                Session("filter") = Nothing
                Session("Column") = Nothing
            End If
            pnlSearch.Visible = False
            btnSearch.Visible = False
            btnExpand.Visible = False
            ddlField.Visible = False
            tbFilter.Visible = False
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


    Protected Sub Btnclosing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClosing.Click
        Dim SQLString, StrFilter As String
        Dim Result As String
        Try
            SQLString = "EXEC S_PLLKMHarianClosingView '" + ddlStartWeek.SelectedValue + "', '" + ddlEndWeek.SelectedValue + "','1', '" + ddlDivision.SelectedValue + "' " + StrFilter
            Result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
            If Result.Length > 5 Then
                MovePanel(pnlInput, pnlInput)
                PnlMain.Visible = False
                StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

                SQLString = "Declare @A VarChar(255) EXEC S_PLLKMHarianClosingPost '" + ddlStartWeek.SelectedValue + "', '" + ddlEndWeek.SelectedValue + "','@FgFlag', @A Out SELECT @A"
            Else
                lbstatus.Text = "Closing Success.!" 'MsgBox(, MsgBoxStyle.Information, "Alert")
                'lbstatus.Text = ""
                Exit Sub
            End If
        Catch ex As Exception
            lbstatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try
            MovePanel(pnlInput, pnlService)
            PnlMain.Visible = True
        Catch ex As Exception
            lbstatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim SQLString As String
        Dim Result As String
        SQLString = "Declare @A VarChar(255) EXEC S_PLLKMHarianClosingPost '" + ddlStartWeek.SelectedValue + "', '" + ddlEndWeek.SelectedValue + "','@FgFlag', @A Out SELECT @A"
        Result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
        If Result.Length > 5 Then
            lbstatus.Text = Result
            Exit Sub
        End If
    End Sub


    Protected Sub btnUnClosing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnUnClosing.Click
        Dim SQLString As String
        Dim Result As String

        SQLString = "Declare @A VarChar(255) EXEC S_PLLKMHarianClosingUnPost '" + ddlStartWeek.SelectedValue + "', '" + ddlEndWeek.SelectedValue + "','@UserId', @A Out SELECT @A"
        Result = SQLExecuteScalar(SQLString, ViewState("DBConnection"))
        If Result.Length > 5 Then
            lbstatus.Text = Result
        Else
            lbstatus.Text = "Un-Closing Success" 'MsgBox("Un-Closing Success", MsgBoxStyle.Information, "Alert")
            'lbstatus.Text = ""
            Exit Sub
        End If
    End Sub


    Protected Sub ddlEndWeek_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEndWeek.SelectedIndexChanged
        Dim StrFilter, SqlString As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)

            SqlString = "EXEC S_PLLKMHarianClosingView '" + ddlStartWeek.SelectedValue + "', '" + ddlEndWeek.SelectedValue + "','1', '" + ddlDivision.SelectedValue + "' " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "EmpID ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
            MovePanel(pnlinput, pnlService)
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub



    Protected Sub ddlDivision_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivision.SelectedIndexChanged
        Dim StrFilter, SqlString As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "EXEC S_PLLKMHarianClosingView '" + ddlStartWeek.SelectedValue + "', '" + ddlEndWeek.SelectedValue + "','1', '" + ddlDivision.SelectedValue + "' " + StrFilter


            'If ViewState("SortExpression") = Nothing Then
            '    ViewState("SortExpression") = "EmpID ASC"
            'End If
            'BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
            MovePanel(pnlInput, pnlService)
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub


    Function CheckMenuLevel(ByVal CommandName As String) As Boolean
        Try
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbstatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function


    Private Sub BindData(Optional ByVal AdvanceFilter As String = "")
        Dim StrFilter, SqlString As String
        Dim GVR As GridViewRow
        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            SqlString = "EXEC S_PL LKMHarianClosingView '" + ddlStartWeek.SelectedValue + "', '" + ddlEndWeek.SelectedValue + "','1', '" + ddlDivision.SelectedValue + "' " + StrFilter
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "EmpID ASC"
                ViewState("SortOrder") = "ASC"
            End If
            BindDataMaster(SqlString, DataGrid, ViewState("SortExpression"), ViewState("DBConnection"))
            GVR = DataGrid.FooterRow
        Catch ex As Exception
            lbstatus.Text = lbstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try

    End Sub

    Protected Sub btnExpand_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Try
            tbfilter2.Text = ""
            If pnlSearch.Visible Then
                pnlSearch.Visible = False
            Else
                pnlSearch.Visible = True
            End If
        Catch ex As Exception
            lbstatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
    End Sub


    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            DataGrid.PageIndex = 0
            DataGrid.EditIndex = -1
            BindData()
        Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
        End Try
    End Sub


    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As EventArgs)
        '    Dim code As String
        '    Dim qrGenerator As New QRCodeEncoder()
        '    Dim qrCode As QRCodeEncoder.ENCODE_MODE = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q)
        '    Dim imgBarCode As New System.Web.UI.WebControls.Image()
        '    code = txtCode.Text
        '    imgBarCode.Height = 150
        '    imgBarCode.Width = 150
        '    Using bitMap As Bitmap = qrCode.GetGraphic(20)
        '        Using ms As New MemoryStream()
        '            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        '            Dim byteImage As Byte() = ms.ToArray()
        '            imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage)
        '        End Using
        '        plBarCode.Controls.Add(imgBarCode)
        '        'End Using                         
    End Sub



End Class
