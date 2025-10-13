Imports System.Data
'Imports System.Windows.Forms

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim ConnString As String = System.Configuration.ConfigurationManager.AppSettings.Get("DBConnection") + ";User ID=userlicense;Password=2580456;Connection Timeout=600"

    'Server Live
    'Dim ConnString As String = System.Configuration.ConfigurationManager.AppSettings.Get("DBConnection") + ";User ID=DB9758_userlicense;Password=XEyb*sT3Ek;Connection Timeout=600"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                ''Session.Clear()
                dbUser.Focus()
                lStatus.Text = ""
                dbUser.Attributes.Add("autocomplete", "off")
                dbPassword.Attributes.Add("autocomplete", "off")
                FillCombo(ddlServer, "Select Company, DbName From MsCompany", False, "DbName", "Company", ConnString)
            End If
            'lStatus.Text = ""
        Catch ex As Exception
            lStatus.Text = "Page Load Error : " + ex.ToString
        End Try
    End Sub

    'Public ReadOnly Property HiddenValue2() As String
    '    Get
    '        Return hdpinstance.Value
    '    End Get
    'End Property

    Protected Sub bSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bSubmit.Click
        Dim dt As DataTable
        Dim dr As DataRow 'dr2 
        Dim UserProperti As DataRow 'Digit
        Dim ServerIP, Company As String
        Dim ExpireDb As String

       
        Try
            Company = SQLExecuteScalar("SELECT * FROM MsCompany WHERE Company = " + QuotedStr(ddlServer.Text) + "", ConnString)
            'lStatus.Text = Company
            'Exit Sub
            If dbUser.Text = "" Then
                lStatus.Text = "User Name Must Be Filled."
                dbUser.Focus()
            ElseIf dbPassword.Text = "" Then
                lStatus.Text = "Password Must Be Filled."
                dbPassword.Focus()
                'ElseIf ddlServer.Text = "" Then
                '    lStatus.Text = "Company Must Be Filled."
                '    ddlServer.Focus()
                'ElseIf Company = "" Then
                '    lStatus.Text = "Company is not Correct, Please try again"
                '    ddlServer.Focus()
            Else
                'dt = SQLExecuteQuery("EXEC S_SAUserLogon " + QuotedStr(dbUser.Text) + ", " + QuotedStr(dbPassword.Text) + ", '" + ddlServer.Text + "'", ConnString).Tables(0)
                dt = SQLExecuteQuery("EXEC S_SAUserLogon " + QuotedStr(dbUser.Text) + ", " + QuotedStr(dbPassword.Text) + ", '" + ddlServer.SelectedValue + "'", ConnString).Tables(0)
                If dt.Rows(0)("Msg").ToString <> "" Then
                    lStatus.Text = dt.Rows(0)("Msg").ToString
                Else
                    dr = dt.Rows(0)
                    'Password = dt.Rows(0)("Pwd").ToString
                    'If String.Compare(Password, dbPassword.Text) = 0 Then

                    ''Session("UserId") = dbUser.Text
                    ''Session("UserName") = dr("UserName").ToString
                    ''Session("FgAdmin") = dr("FgAdmin").ToString
                    ''dr2 = SQLExecuteQuery("EXEC S_SAPeriodInfo -1,-1," + QuotedStr(ddlServer.SelectedItem.Text)).Tables(0).Rows(0)
                    ''Session("PeriodInfo") = dr2
                    ''Session("GLYear") = dr2("DefaultYear").ToString
                    ''Session("GLPeriod") = dr2("DefaultPeriod").ToString
                    ''Session("Rate") = dr2("MiddleRate").ToString
                    ''ViewState("Currency") = dr2("Currency").ToString
                    ''Session("CompanyName") = dr2("CompanyName").ToString
                    ''Session("Addr1") = dr2("CompLine1").ToString
                    ''Session("Addr2") = dr2("CompLine2").ToString
                    ''Session("PageSizeGrid") = dr2("PageSizeGrid").ToString
                    ''Session("ServerDate") = dr2("ServerDate")

                    Dim GuidID As Guid
                    Dim KeyID As String
                    GuidID = Guid.NewGuid
                    KeyID = GuidID.ToString.Substring(0, 6)

                    '[0] = DigitHome, 1 = DigitRate, 2 = digitQty, 3= DigitPercent
                    ''Digit = SQLExecuteQuery("SELECT dbo.DigitCurrRate() AS Rate, dbo.DigitCurrHome() AS Home, dbo.DigitPercent() AS [Percent], dbo.DigitQty() AS Qty").Tables(0).Rows(0)
                    ''Session("Digit") = Digit

                    ServerIP = System.Configuration.ConfigurationManager.AppSettings.Get("ServerIP")

                    'UserProperti = SQLExecuteQuery("EXEC S_SAUserProperti '" + dbUser.Text.Trim + "', '" + ServerIP.Trim + "', '" + ddlServer.Text + "'", ConnString).Tables(0).Rows(0)
                    UserProperti = SQLExecuteQuery("EXEC S_SAUserProperti '" + dbUser.Text.Trim + "', '" + ServerIP.Trim + "', '" + ddlServer.SelectedItem.Text + "'", ConnString).Tables(0).Rows(0)
                    Session(KeyID) = UserProperti
                    

                    ExpireDb = TrimStr(Session(KeyID)("CompanyDB").ToString)
                    If ExpireDb = "" Then
                        lStatus.Text = "Login Failed... Error code 100, Please contact dataprima..."
                        Exit Sub
                    Else
                        Dim StrHari, StrDay, StrBulan, StrTahun As String
                        Dim iTahun, iBulan, iHari, Iday, EHari As Integer
                        Dim dateexpire, datenow As DateTime
                        StrBulan = Mid(ExpireDb, 4, 2)
                        StrTahun = Mid(ExpireDb, 8, 2)
                        StrHari = Mid(ExpireDb, 7, 1)
                        StrDay = Mid(ExpireDb, 6, 1)
                        iHari = Asc(StrHari) - 65
                        Iday = Asc(StrDay) - 64
                        iTahun = CInt(StrTahun) - 5
                        iTahun = iTahun + 2000
                        iBulan = CInt(StrBulan) - 6
                        datenow = TrimStr(Session(KeyID)("DateSJ"))
                        dateexpire = New DateTime(iTahun, iBulan, Iday)
                        EHari = dateexpire.DayOfWeek
                        If iHari <> EHari Then
                            lStatus.Text = "Login Failed... Error code 101. Data damage, Please contact dataprima..."
                            Exit Sub
                        End If
                        'lStatus.Text = "Expire : " + FormatDateTime(dateexpire, DateFormat.LongDate) + "   Hr Ini : " + FormatDateTime(datenow, DateFormat.LongDate)
                        'Exit Sub
                        'If dateexpire <= datenow Then
                        '    lStatus.Text = "Login Failed... Error code 102, Please contact dataprima..."
                        '    Exit Sub
                        'End If
                    End If


                    FormsAuthentication.RedirectFromLoginPage(dbUser.Text, False)
                    'Session("DBConnection") = "Data Source=" + ServerIP + ";Initial Catalog=" + ddlServer.Text + ";Persist Security Info=True;Connect Timeout=600;User ID=userlicense;Password=2580456"
                    'Application("DBConnection") = "Data Source=" + ServerIP + ";Initial Catalog=" + ddlServer.Text + ";Persist Security Info=True;Connect Timeout=600;User ID=userlicense;Password=2580456"
                    'Session("DBMaster") = "Data Source=" + ServerIP + ";Initial Catalog=ASPAC;Persist Security Info=True;Connection Timeout=600;User ID=userlicense;Password=2580456"
                    
                    Response.Redirect("Index.aspx?KeyId=" + KeyID.ToString)
                End If
            End If
        Catch ex As Exception
            lStatus.Text = "Btn Submit Error : " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub bReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bReset.Click
        lStatus.Text = ""
        dbUser.Text = ""
        dbPassword.Text = ""
    End Sub
End Class

