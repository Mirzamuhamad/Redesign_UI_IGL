Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data

Partial Class Transaction_TrCompInfo_TrCompInfo
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
            'btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            tbEffectiveDate_SelectionChanged(Nothing, Nothing)
            tbEffectiveDate2_SelectionChanged(Nothing, Nothing)
            tbEffectiveDate3_SelectionChanged(Nothing, Nothing)
            pnlInput.Visible = True
            MultiView1.ActiveViewIndex = 0
            Menu1.Items.Item(0).Selected = True
            ViewState("EditHd") = False
            Dim dt As DataTable
            dt = SQLExecuteQuery("Select * from SACompany ", ViewState("DBConnection").ToString).Tables(0)
            If dt.Rows.Count > 0 Then
                tbCompanyName.Text = dt.Rows(0)("CompanyName").ToString
                tbAddr1.Text = dt.Rows(0)("Address1").ToString
                tbAddr2.Text = dt.Rows(0)("Address2").ToString
                tbCity.Text = dt.Rows(0)("City").ToString
                tbCountry.Text = dt.Rows(0)("Country").ToString
                tbZipCode.Text = dt.Rows(0)("ZipCode").ToString
                tbPhone.Text = dt.Rows(0)("Phone").ToString
                tbFax.Text = dt.Rows(0)("Fax").ToString
                tbBranchAddr1.Text = dt.Rows(0)("BranchAddr1").ToString
                tbBranchAddr2.Text = dt.Rows(0)("BranchAddr2").ToString
                tbBranchCity.Text = dt.Rows(0)("BranchCity").ToString
                tbBranchCountry.Text = dt.Rows(0)("BranchCountry").ToString
                tbBranchZipCode.Text = dt.Rows(0)("BranchZipCode").ToString
                tbBranchPhone.Text = dt.Rows(0)("BranchPhone").ToString
                tbBranchFax.Text = dt.Rows(0)("BranchFax").ToString
                tbEmail.Text = dt.Rows(0)("Email").ToString
                tbWeb.Text = dt.Rows(0)("Web").ToString
                tbNpwp.Text = dt.Rows(0)("NPWP").ToString
                tbNpwp2.Text = dt.Rows(0)("NPWP2").ToString
                tbNpwp3.Text = dt.Rows(0)("NPWP3").ToString
                tbNpwp4.Text = dt.Rows(0)("NPWP4").ToString
                tbNPKP.Text = dt.Rows(0)("nppkp").ToString
                tbMoto.Text = dt.Rows(0)("Moto").ToString
                tbNonReport.Text = dt.Rows(0)("coynonreport").ToString
                tbReport.Text = dt.Rows(0)("coyreport").ToString
                BindToDate(tbEffectiveDate, dt.Rows(0)("EffectiveNPWP").ToString)
                BindToDate(tbEffectiveDate2, dt.Rows(0)("EffectiveNPWP2").ToString)
                BindToDate(tbEffectiveDate3, dt.Rows(0)("EffectiveNPWP3").ToString)
                BindToDate(tbEffectiveDate4, dt.Rows(0)("EffectiveNPWP4").ToString)
                ViewState("EditHd") = True
            End If
        End If
        If Not Session("Result") Is Nothing Then
            Session("Result") = Nothing
            ViewState("Sender") = Nothing
            Session("filter") = Nothing
            Session("Criteria") = Nothing
            Session("Column") = Nothing
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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Try
            If tbCompanyName.Text.Trim = "" Then
                lstatus.Text = "Company Must Have Value."
                tbCompanyName.Focus()
                Exit Sub
            End If
            If tbEffectiveDate.IsNull And tbNpwp.Text.Trim <> "" Then
                lstatus.Text = "Effective Date Must Have Value."
                tbEffectiveDate.Focus()
                Exit Sub
            End If
            If tbEffectiveDate3.IsNull And tbNpwp3.Text.Trim <> "" Then
                lstatus.Text = "Effective Date 3 Must Have Value."
                tbEffectiveDate3.Focus()
                Exit Sub
            End If
            If tbEffectiveDate4.IsNull And tbNpwp4.Text.Trim <> "" Then
                lstatus.Text = "Effective Date 4 Must Have Value."
                tbEffectiveDate4.Focus()
                Exit Sub
            End If
            If ViewState("EditHd") = True Then
                SQLString = "Update SACompany set CompanyName= " + QuotedStr(tbCompanyName.Text) + "," & _
            "Address1 = " + QuotedStr(tbAddr1.Text) + ", Address2 = " + QuotedStr(tbAddr2.Text) + ", " + _
            "City = " + QuotedStr(tbCity.Text) + ", Country = " + QuotedStr(tbCountry.Text) + ", " + _
            "zipcode = " + QuotedStr(tbZipCode.Text) + ", phone = " + QuotedStr(tbPhone.Text) + ", " + _
            "fax = " + QuotedStr(tbFax.Text) + ", branchaddr1 = " + QuotedStr(tbBranchAddr1.Text) + ", " + _
            "branchaddr2 = " + QuotedStr(tbBranchAddr2.Text) + ", branchCity = " + QuotedStr(tbBranchCity.Text) + ", " + _
            "branchCountry = " + QuotedStr(tbBranchCountry.Text) + ", branchzipcode = " + QuotedStr(tbBranchZipCode.Text) + ", " + _
            "branchphone = " + QuotedStr(tbBranchCountry.Text) + ", branchfax = " + QuotedStr(tbBranchFax.Text) + ", " + _
            "email = " + QuotedStr(tbEmail.Text) + ", web = " + QuotedStr(tbWeb.Text) + ", " + _
            "npwp = " + QuotedStr(tbNpwp.Text) + ", nppkp = " + QuotedStr(tbNPKP.Text) + ", " + _
            "moto = " + QuotedStr(tbMoto.Text) + ", coynonreport = " + QuotedStr(tbNonReport.Text) + ", " + _
            "coyreport = " + QuotedStr(tbReport.Text) + ", EffectiveNPWP = '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") + "', " + _
            " EffectiveNPWP2 = '" + Format(tbEffectiveDate2.SelectedValue, "yyyy-MM-dd") + "', " + _
            " EffectiveNPWP3 = '" + Format(tbEffectiveDate3.SelectedValue, "yyyy-MM-dd") + "', " + _
            " EffectiveNPWP4 = '" + Format(tbEffectiveDate4.SelectedValue, "yyyy-MM-dd") + "', " + _
            " NPWP2 = " + QuotedStr(tbNpwp2.Text) + ", " + _
            " NPWP3 = " + QuotedStr(tbNpwp3.Text) + ", " + _
            " NPWP4 = " + QuotedStr(tbNpwp4.Text)
            Else
                SQLString = "INSERT INTO SACompany (CompanyName, Address1, Address2, City, Country, zipcode, phone, fax, branchaddr1, branchaddr2, " + _
                "branchcity, branchcountry, branchzipcode, branchphone, branchfax, email, web, npwp, nppkp, moto, coynonreport, NPWP2, NPWP3, NPWP4, EffectiveNPWP, EffectiveNPWP2, EffectiveNPWP3, EffectiveNPWP4, coyreport)" + _
                "VALUES (" + QuotedStr(tbCompanyName.Text) + ", " + QuotedStr(tbAddr1.Text) + ", " + QuotedStr(tbAddr2.Text) + ", " + _
                QuotedStr(tbCity.Text) + ", " + QuotedStr(tbCountry.Text) + ", " + _
                QuotedStr(tbZipCode.Text) + ", " + QuotedStr(tbPhone.Text) + ", " + _
                QuotedStr(tbFax.Text) + ", " + QuotedStr(tbBranchAddr1.Text) + ", " + _
                QuotedStr(tbBranchAddr2.Text) + ", " + QuotedStr(tbBranchCity.Text) + ", " + _
                QuotedStr(tbBranchCountry.Text) + ", " + QuotedStr(tbBranchZipCode.Text) + ", " + _
                QuotedStr(tbBranchCountry.Text) + ", " + QuotedStr(tbBranchFax.Text) + ", " + _
                QuotedStr(tbEmail.Text) + ", " + QuotedStr(tbWeb.Text) + ", " + _
                QuotedStr(tbNpwp.Text) + ", " + QuotedStr(tbNPKP.Text) + ", " + _
                QuotedStr(tbMoto.Text) + ", " + QuotedStr(tbNonReport.Text) + ", " + _
                QuotedStr(tbNpwp2.Text) + ", " + QuotedStr(tbNpwp3.Text) + ", " + _
                QuotedStr(tbNpwp4.Text) + ", '" + Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd").Replace("&nbsp;", "") + "', '" + _
                Format(tbEffectiveDate2.SelectedValue, "yyyy-MM-dd").Replace("&nbsp;", "") + "', '" + Format(tbEffectiveDate3.SelectedValue, "yyyy-MM-dd").Replace("&nbsp;", "") + "', '" + _
                Format(tbEffectiveDate4.SelectedValue, "yyyy-MM-dd").Replace("&nbsp;", "") + "', " + _
                QuotedStr(tbReport.Text) + ")"
            End If

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlInput.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, _
              ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        'Make the selected menu item reflect the correct imageurl

        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
                'Menu1.Items(i).ImageUrl = "selectedtab.gif"
            Else
                ' Menu1.Items(i).ImageUrl = "unselectedtab.gif"
            End If
        Next
    End Sub

    Protected Sub tbEffectiveDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEffectiveDate.SelectionChanged
        If tbNpwp.Text.Trim <> "" And Format(tbEffectiveDate.SelectedValue, "yyyy-MM-dd") <> "" Then
            tbNpwp2.Enabled = True
            tbEffectiveDate2.Enabled = True
        Else
            tbNpwp2.Enabled = False
            tbEffectiveDate2.Enabled = False
        End If
    End Sub

    Protected Sub tbEffectiveDate2_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEffectiveDate2.SelectionChanged
        If tbNpwp2.Text.Trim <> "" And Format(tbEffectiveDate2.SelectedValue, "yyyy-MM-dd") <> "" Then
            tbNpwp3.Enabled = True
            tbEffectiveDate3.Enabled = True
        Else
            tbNpwp3.Enabled = False
            tbEffectiveDate3.Enabled = False
        End If
        
    End Sub

    Protected Sub tbEffectiveDate3_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEffectiveDate3.SelectionChanged
        If tbNpwp3.Text.Trim <> "" And Format(tbEffectiveDate3.SelectedValue, "yyyy-MM-dd") <> "" Then
            tbNpwp4.Enabled = True
            tbEffectiveDate4.Enabled = True
        Else
            tbNpwp4.Enabled = False
            tbEffectiveDate4.Enabled = False
        End If
    End Sub
End Class
