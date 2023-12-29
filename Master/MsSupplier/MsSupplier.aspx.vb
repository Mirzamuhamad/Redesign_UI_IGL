Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Master_MsSupplier_MsSupplier
    Inherits System.Web.UI.Page

    Protected con As New SqlConnection
    Protected da, da1 As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * From VMsSupplier"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Try
            If Not IsPostBack Then
                InitProperty()
                ViewState("SortExpression") = Nothing
                FillCombo(ddlSuppType, "SELECT SuppTypeCode, SuppTypeName From MsSuppType ORDER By SuppTypeName", True, "SuppTypeCode", "SuppTypeName", ViewState("DBConnection").ToString)
                FillCombo(ddlSuppClass, "Select SuppClassCode, SuppClassName From MsSuppClass", True, "SuppClassCode", "SuppClassName", ViewState("DBConnection").ToString)
                FillCombo(ddlCurr, "SELECT CurrCode From MsCurrency", False, "CurrCode", "CurrCode", ViewState("DBConnection").ToString)
                FillCombo(ddlCountry, "SELECT CountryCode, CountryName From MsCountry", True, "CountryCode", "CountryName", ViewState("DBConnection").ToString)
                FillCombo(ddlCity, "SELECT CityCode, CityName From MsCity", True, "CityCode", "CityName", ViewState("DBConnection").ToString)
                FillCombo(ddlTerm, "SELECT TermCode, TermName From MsTerm", True, "TermCode", "TermName", ViewState("DBConnection").ToString)
                FillCombo(ddlBank, "SELECT BankCode, BankName From MsBank", True, "BankCode", "BankName", ViewState("DBConnection").ToString)
                FillCombo(ddlWrhsSubkon, "SELECT Wrhs_Code, Wrhs_Name FROM VMsWarehouse WHERE Wrhs_Type = 'Deposit Out' AND FgSupplier = 'Y'", True, "Wrhs_Code", "Wrhs_Name", ViewState("DBConnection").ToString)
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAdd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddDt.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddDt2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddBank.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnPrint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
                SetInit()
            End If
            lbStatus.Text = ""

            If Not Session("StrResult") Is Nothing Then
                BindData(2, Session("StrResult"))
                Session("StrResult") = Nothing
            End If

            If Not Session("Result") Is Nothing Then
                If ViewState("Sender") = "btnGetDt" Then
                    Dim Row, AllRow As DataRow()
                    Dim drResult As DataRow

                    If IsNothing(Session("Result")) Then
                        lbStatus.Text = "Session is empty"
                        Exit Sub
                    End If

                    For Each drResult In Session("Result").Rows
                        AllRow = ViewState("Dt").Select()
                        Row = ViewState("Dt").select("ContactName = " + QuotedStr(drResult("ContactName")))
                        If Row.Length = 0 Then
                            'insert
                            Dim dr As DataRow
                            dr = ViewState("Dt").NewRow
                            If AllRow.Length = 0 Then
                                dr("ItemNo") = "1"
                            Else
                                dr("ItemNo") = (CInt(AllRow(AllRow.Length - 1)("ItemNo").ToString) + 1).ToString
                            End If
                            dr("ContactName") = drResult("ContactName")
                            dr("AccountName") = drResult("Description")
                            dr("Address1") = ""
                            dr("Country") = DBNull.Value
                            dr("ContactTitle") = drResult("ContactTitle")
                            dr("ZipCode") = ""
                            dr("CurrCode") = drResult("Currency")
                            dr("Phone") = 0
                            dr("Fax") = 0
                            dr("Email") = 0
                            ViewState("Dt").Rows.Add(dr)
                        Else
                            'edit
                            Row(0).BeginEdit()
                            Row(0)("ContactName") = drResult("ContactName")
                            Row(0)("AccountName") = drResult("description")
                            Row(0)("ContactTitle") = drResult("ContactTitle")
                            Row(0)("CurrCode") = drResult("Currency")
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
                Session("Criteria") = Nothing
                Session("Column") = Nothing
            End If
            tbNPWP.Enabled = ddlPKP.SelectedIndex = 0
            ddlWrhsSubkon.Enabled = ddlFgSubkon.SelectedIndex = 0
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
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If
            If CommandName = "Edit" Then
                If ViewState("MenuLevel").Rows(0)("FgEdit") = "N" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to edit record. Please contact administrator')}</script>"
                    Return False
                    Exit Function
                End If
            End If

            If CommandName = "Delete" Then
                If ViewState("MenuLevel").Rows(0)("FgDelete") = "N" Then
                    lbStatus.Text = "<script language='javascript'> {alert('You are not authorized to delete record. Please contact administrator')}</script>"
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
                        StrFilter = " WHERE " + ddlField.SelectedValue + " like '%" + tbFilter.Text + "%' " + _
                        ddlNotasi.SelectedValue + " " + ddlField2.SelectedValue + " like '%" + tbfilter2.Text + "%'"
                    Else
                        StrFilter = " WHERE " + ddlField.SelectedValue + " like '%" + tbFilter.Text + "%'"
                    End If
                Else
                    StrFilter = ""
                End If
            End If
            DS = SQLExecuteQuery("Select A.*, B.SuppClassName, C.SuppTypeName, D.CityName, E.TermName, T.Wrhs_Name As WrhsSubkonName FROM MsSupplier A LEFT OUTER JOIN MsSuppClass B ON A.SuppClass = B.SuppClassCode LEFT OUTER JOIN MsSuppType C ON A.SuppType = C.SupptypeCode LEFT OUTER JOIN MsCity D ON A.City = D.CityCode LEFT OUTER JOIN MsTerm E ON A.Term = E.TermCode " + _
                                 " LEFT OUTER JOIN VMsWarehouse T ON A.WrhsSubkon = T.Wrhs_Code " + StrFilter, ViewState("DBConnection").ToString)

            If DS.Tables(0).Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
            End If

            DV = DS.Tables(0).DefaultView
            DV.Sort = ViewState("SortExpression")
            GridView1.DataSource = DV
            GridView1.DataBind()

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            BindData(1)
            pnlNav.Visible = True
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Private Sub ModifyInput(ByVal State As Boolean)
        Try
            tbCode.Enabled = State
            tbName.Enabled = State
            ddlSuppClass.Enabled = State
            ddlSuppType.Enabled = State
            tbAddress1.Enabled = State
            tbAddress2.Enabled = State
            ddlCity.Enabled = State
            ddlCurr.Enabled = State
            tbTerm.Enabled = State
            ddlTerm.Enabled = State
            tbMemberOfGroup.Enabled = State
            tbSisterCompany.Enabled = State
            tbAddress1.Enabled = State
            tbAddress2.Enabled = State
            tbPhone.Enabled = State
            ddlCity.Enabled = State
            tbPostCode.Enabled = State
            tbFax.Enabled = State
            tbEmail.Enabled = State
            ddlPKP.Enabled = State
            ddlPPN.Enabled = State
            ddlAktive.Enabled = State
            tbISOCertNo.Enabled = State
            tbSIUPNo.Enabled = State
            tbDirectorName.Enabled = State
            tbContactPerson.Enabled = State
            tbContactTitle.Enabled = State
            tbContactPh.Enabled = State
            ddlFgSubkon.Enabled = State
            ddlWrhsSubkon.Enabled = State
            btnSaveHd.Enabled = State
            btnCancelHd.Enabled = State
            tbNoKTPHd.Enabled = State
            btnReset.Enabled = State
            tbNPWP.Enabled = State And ddlPKP.SelectedIndex = 0
            ddlWrhsSubkon.Enabled = State And ddlFgSubkon.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Modify Input Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub EnableHd(ByVal State As Boolean)
        Try
            tbCode.Enabled = State
            'tbName.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Function GetStringDt2(ByVal Nmbr As String) As String
        Return "SELECT SuppCode, DeliveryCode, DeliveryPlace, Address1, Address2, ZipCode, Phone, Fax, Email, ContactPerson, ContactTitle, ContactHP, Remark, UserId, UserDate FROM MsSuppAddress WHERE SuppCode = '" + Nmbr + "'"
    End Function

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT SuppCode, ItemNo, ContactName, ContactTitle, Address1, Address2, Country, ZipCode, Phone, Fax, Email,NoKtp, UserID, UserDate FROM MsSuppContact WHERE SuppCode = '" + Nmbr + "'"
    End Function

    Private Function GetStringDt3(ByVal Nmbr As String) As String
        Return "SELECT A.SuppCode, A.Item, A.Bank, B.BankName, A.AccountName, A.AccountNo, A.SwiftCode, A.Branch, A.UserId, A.UserDate FROM MsSuppBank A LEFT OUTER JOIN MsBank B ON A.Bank = B.BankCode WHERE SuppCode = '" + Nmbr + "'"
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

    Private Sub BindDataDt2(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt2") = Nothing
            dt = SQLExecuteQuery(GetStringDt2(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt2") = dt
            GridViewAddr.DataSource = dt
            GridViewAddr.DataBind()
            BindGridDt(dt, GridViewAddr)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt2 Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDt3(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt3") = Nothing
            dt = SQLExecuteQuery(GetStringDt3(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("Dt3") = dt
            GridDt3.DataSource = dt
            GridDt3.DataBind()
            BindGridDt(dt, GridDt3)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt3 Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Try
            pnlView.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt.Click
        Try
            pnlEditDt.Visible = False
            pnlDt.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            SaveDt1()

        Catch ex As Exception
            lbStatus.Text = "Save contact Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveAll()
        Dim SQLString As String
        Dim I As Integer
        Try
            If pnlDt.Visible = False Then
                lbStatus.Text = "Detail Data must be saved first"
                Exit Sub
            End If
            'Save Hd
            If ViewState("StateHd") = "Insert" Then
                SQLString = "INSERT INTO MsSupplier (suppcode, suppname, memberofgroup, sistercompany, supptype, suppclass, address1, address2, city, zipcode, phone, fax, email, currcode, term, npwp, fgppn, nppkp, ownername, contactperson, contacttitle, contacthp, isocertno, siupno, fgsubkon, wrhssubkon,NoKTP, fgactive, userid, userdate) " + _
                "SELECT '" + tbCode.Text + "', '" + tbName.Text + "', '" + tbMemberOfGroup.Text + "', '" + tbSisterCompany.Text + "', '" + ddlSuppClass.SelectedValue + "', '" + ddlSuppType.SelectedValue + "', '" + tbAddress1.Text + "', '" + tbAddress2.Text + "'," + _
                "'" + ddlCity.SelectedValue + " ', '" + tbPostCode.Text + "', '" + tbPhone.Text + "', '" + tbFax.Text + "', '" + tbEmail.Text + "', '" + ddlCurr.SelectedValue + "', '" + ddlTerm.SelectedValue + "'," + _
                "'" + tbNPWP.Text + " ', '" + ddlPPN.SelectedValue + "', '" + ddlPKP.SelectedValue + "', '" + tbDirectorName.Text + "', '" + tbContactPerson.Text + "'," + _
                "'" + tbContactTitle.Text + " ', '" + tbContactPh.Text + "', '" + tbISOCertNo.Text + +"', '" + tbSIUPNo.Text + "', '" + ddlFgSubkon.SelectedValue + "', '" + ddlWrhsSubkon.SelectedValue + "','" + tbNoKTPHd.Text + "', '" + ddlAktive.SelectedValue + "', '" + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate(), "
            Else
                SQLString = "UPDATE MsSupplier SET SuppClass = " + QuotedStr(ddlSuppClass.SelectedValue) + ", " + _
                " supptype = " + QuotedStr(ddlSuppType.SelectedValue) + ", Address1 = " + QuotedStr(tbAddress1.Text) + ", " + _
                " Address2 = " + QuotedStr(tbAddress2.Text) + ", City = " + QuotedStr(ddlCity.SelectedValue) + ", ZipCode = " + QuotedStr(tbPostCode.Text) + ", " + _
                " Phone = " + QuotedStr(tbPhone.Text) + ", Fax = " + QuotedStr(tbFax.Text) + ", Email = " + QuotedStr(tbEmail.Text) + ", " + _
                " CurrCode = " + QuotedStr(ddlCurr.SelectedValue) + ", Term = " + QuotedStr(ddlTerm.SelectedValue) + ", npwp = " + QuotedStr(tbNPWP.Text) + ", " + _
                " fgppn = " + QuotedStr(ddlPPN.SelectedValue) + ", nppkp = " + QuotedStr(ddlPKP.SelectedValue) + ", ownername = " + QuotedStr(tbDirectorName.Text) + ", " + _
                " contactperson = " + QuotedStr(tbContactName.Text) + ", contacttitle = " + QuotedStr(tbContactTitle.Text) + ", contacthp = " + QuotedStr(tbContactPh.Text) + ", " + _
                " fgactive = " + QuotedStr(ddlAktive.SelectedValue) + ", " + _
                " memberofgroup = " + QuotedStr(tbMemberOfGroup.Text) + ", " + _
                " sistercompany = " + QuotedStr(tbSisterCompany.Text) + ", " + _
                " isocertno = " + QuotedStr(tbISOCertNo.Text) + ", " + _
                " siupno = " + QuotedStr(tbSIUPNo.Text) + ", " + _
                " fgsubkon = " + QuotedStr(ddlFgSubkon.SelectedValue) + ", " + _
                " wrhssubkon = " + QuotedStr(ddlWrhsSubkon.SelectedValue) + ", " + _
                " NoKTP = " + QuotedStr(tbNoKTPHd.Text) + ", " + _
                " SuppName = " + tbName.Text + "', UserDate = getDate()" + _
                " WHERE SuppCode = '" + tbCode.Text + "'"
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)

            'update Primary Key on Dt
            Dim Row As DataRow()

            Row = ViewState("Dt").Select("Supplier Code IS NULL")
            For I = 0 To Row.Length - 1
                Row(I).BeginEdit()
                Row(I)("SuppCode") = tbCode.Text
                Row(I).EndEdit()
            Next

            'save dt
            Dim ConnString As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
            con = New SqlConnection(ConnString)
            con.Open()
            Dim cmdSql As New SqlCommand("SELECT SuppCode, ItemNo, ContactName, ContactTitle, Address1, Address2, Country, ZipCode, Phone, Fax, Email,NoKtp, UserID, UserDate FROM MsSuppContact WHERE SuppCode = '" & ViewState("SuppCode") & "'", con)
            da1 = New SqlDataAdapter(cmdSql)
            Dim dbcommandBuilder As SqlCommandBuilder = New SqlCommandBuilder(da1)

            da1.InsertCommand = dbcommandBuilder.GetInsertCommand
            da1.DeleteCommand = dbcommandBuilder.GetDeleteCommand
            da1.UpdateCommand = dbcommandBuilder.GetUpdateCommand

            Dim Dt As New DataTable("MsSuppContact")

            Dt = ViewState("Dt")
            da1.Update(Dt)
            Dt.AcceptChanges()
            ViewState("Dt") = Dt
        Catch ex As Exception
            Throw New Exception("Save All Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub SaveHd()
        Dim SQLString As String
        Try
            If tbCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Code Must Have Value")
                tbCode.Focus()
                Exit Sub
            End If
            If tbName.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Name Must Have Value")
                tbName.Focus()
                Exit Sub
            End If
            If ddlSuppType.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier Type Must Have Value")
                ddlSuppType.Focus()
                Exit Sub
            End If
            If ddlSuppClass.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier Class Must Have Value")
                ddlSuppClass.Focus()
                Exit Sub
            End If
            If ddlCurr.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Supplier Class Must Have Value")
                ddlCurr.Focus()
                Exit Sub
            End If
            If ddlTerm.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Payment Term Must Have Value")
                ddlTerm.Focus()
                Exit Sub
            End If
            If ddlFgSubkon.SelectedValue = "Y" Then
                If ddlWrhsSubkon.Text.Trim = "" Then
                    lbStatus.Text = MessageDlg("Warehouse Subcont Must Have Value")
                    ddlWrhsSubkon.Focus()
                    Exit Sub
                End If
            End If
            If IsNumeric(tbPostCode.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Postal Code must be in numeric.")
                tbPostCode.Focus()
                Exit Sub
            End If
            If IsNumeric(tbFax.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Fax must be in numeric.")
                tbFax.Focus()
                Exit Sub
            End If
            If IsNumeric(tbPhone.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Telephone must be in numeric.")
                tbPhone.Focus()
                Exit Sub
            End If
            If IsNumeric(tbContactPh.Text.Replace(",", "")) = 0 Then
                lbStatus.Text = MessageDlg("Contact Phone must be in numeric.")
                tbContactPh.Focus()
                Exit Sub
            End If
            If ViewState("StateHd") = "Insert" Then
                Dim dt As DataTable
                SQLString = "SELECT SuppCode FROM MsSupplier WHERE SuppCode = " + QuotedStr(tbCode.Text)
                dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

                If dt.Rows.Count <> 0 Then
                    lbStatus.Text = MessageDlg("Supplier Code already exists")
                    tbCode.Focus()
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsSupplier (suppcode, suppname,  memberofgroup, sistercompany, suppclass, supptype, address1, address2, city, zipcode, phone, fax, email, currcode, term, npwp, fgppn, nppkp, ownername, contactperson, contacttitle, contacthp, isocertno, siupno, fgsubkon, wrhssubkon, NoKTP, fgactive, userid, userdate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + ", " + QuotedStr(tbMemberOfGroup.Text) + ", " + QuotedStr(tbSisterCompany.Text) + ", " + QuotedStr(ddlSuppClass.SelectedValue) + ", " + QuotedStr(ddlSuppType.SelectedValue) + ", " + QuotedStr(tbAddress1.Text) + ", " + QuotedStr(tbAddress2.Text) + ", " + _
                QuotedStr(ddlCity.SelectedValue) + ", " + QuotedStr(tbPostCode.Text) + ", " + QuotedStr(tbPhone.Text) + ", " + QuotedStr(tbFax.Text) + ", " + QuotedStr(tbEmail.Text) + ", " + QuotedStr(ddlCurr.SelectedValue) + ", " + QuotedStr(ddlTerm.SelectedValue) + ", " + _
                QuotedStr(tbNPWP.Text) + ", " + QuotedStr(ddlPPN.SelectedValue) + ", " + QuotedStr(ddlPKP.SelectedValue) + ", " + QuotedStr(tbDirectorName.Text) + ", " + QuotedStr(tbContactPerson.Text) + ", " + _
                QuotedStr(tbContactTitle.Text) + " , " + QuotedStr(tbContactPh.Text) + " , " + QuotedStr(tbISOCertNo.Text) + ", " + QuotedStr(tbSIUPNo.Text) + " , " + QuotedStr(ddlFgSubkon.SelectedValue) + " , " + QuotedStr(ddlWrhsSubkon.SelectedValue) + ", " + QuotedStr(tbNoKTPHd.Text) + ", " + QuotedStr(ddlAktive.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate() "
            Else
                SQLString = "UPDATE MsSupplier SET SuppClass = '" + ddlSuppClass.SelectedValue + "'," + _
                " supptype = " + QuotedStr(ddlSuppType.SelectedValue) + ", address1= " + QuotedStr(tbAddress1.Text) + ", " + _
                " address2 = " + QuotedStr(tbAddress2.Text) + ", city = " + QuotedStr(ddlCity.SelectedValue) + ", " + _
                " zipcode = " + QuotedStr(tbPostCode.Text) + ", phone = " + QuotedStr(tbPhone.Text) + ", " + _
                " Fax = " + QuotedStr(tbFax.Text) + ", email = " + QuotedStr(tbEmail.Text) + ", " + _
                " currcode = " + QuotedStr(ddlCurr.SelectedValue) + ", term = " + QuotedStr(ddlTerm.SelectedValue) + ", " + _
                " npwp = " + QuotedStr(tbNPWP.Text) + ", fgppn = " + QuotedStr(ddlPPN.SelectedValue) + ", " + _
                " nppkp = " + QuotedStr(ddlPKP.SelectedValue) + ", ownername = " + QuotedStr(tbDirectorName.Text) + ", " + _
                " contactperson = " + QuotedStr(tbContactPerson.Text) + ", contacttitle = " + QuotedStr(tbContactTitle.Text) + ", " + _
                " contacthp = " + QuotedStr(tbPhone.Text) + ", " + _
                " fgactive = " + QuotedStr(ddlAktive.SelectedValue) + ", " + _
                " memberofgroup = " + QuotedStr(tbMemberOfGroup.Text) + ", " + _
                " sistercompany = " + QuotedStr(tbSisterCompany.Text) + ", " + _
                " isocertno = " + QuotedStr(tbISOCertNo.Text) + ", " + _
                " siupno = " + QuotedStr(tbSIUPNo.Text) + ", " + _
                " fgsubkon = " + QuotedStr(ddlFgSubkon.SelectedValue) + ", " + _
                " NoKTP = " + QuotedStr(tbNoKTPHd.Text) + ", " + _
                " wrhssubkon = " + QuotedStr(ddlWrhsSubkon.SelectedValue) + ", " + _
                " SuppName = '" + tbName.Text + "', UserDate = getDate()" + _
                " WHERE SuppCode = '" + tbCode.Text + "'"
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlView.Visible = False
            PnlHd.Visible = True
            BindData(1)
        Catch ex As Exception
            Throw New Exception("Save Hd Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub DeleteHd()
        Dim SQLString As String
        Try
            SQLString = "UPDATE MsSupplier SET FgActive = 'N' WHERE SuppCode = '" & ViewState("SuppCode").ToString & "'"

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        Catch ex As Exception
            Throw New Exception("Delete Hd Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click, btnAdd2.Click
        Try
            ViewState("StateHd") = "Insert"
            ClearHd()
            PnlHd.Visible = False
            pnlView.Visible = True
            ModifyInput(True)
            MultiView1.Visible = False
            Menu1.Visible = False
        Catch ex As Exception
            lbStatus.Text = "Btn Add Error : " + ex.ToString
        End Try
    End Sub

    Private Sub newTrans()
        Try
            ClearHd()
            cleardt()
            ViewState("Dt") = SQLExecuteQuery("Select A.* from MsSuppContact A WHERE SuppCode = ''", ViewState("DBConnection").ToString).Tables(0)
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
            ViewState("Dt2") = SQLExecuteQuery("Select A.* from MsSuppAddress A WHERE SuppCode = ''", ViewState("DBConnection").ToString).Tables(0)
            GridViewAddr.DataSource = ViewState("Dt2")
            GridViewAddr.DataBind()
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub
    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbName.Text = ""
            ddlSuppClass.SelectedIndex = 0
            ddlSuppType.SelectedIndex = 0
            tbAddress1.Text = ""
            tbAddress2.Text = ""
            tbPhone.Text = ""
            ddlCity.SelectedIndex = 0
            tbPostCode.Text = ""
            tbFax.Text = ""
            tbEmail.Text = ""
            ddlPKP.SelectedIndex = 1
            tbNPWP.Text = ""
            ddlPPN.SelectedIndex = 1
            ddlAktive.SelectedIndex = 0
            tbMemberOfGroup.Text = ""
            tbSisterCompany.Text = ""
            tbISOCertNo.Text = ""
            tbSIUPNo.Text = ""
            ddlFgSubkon.SelectedValue = "N"
            ddlWrhsSubkon.SelectedValue = ""
            tbDirectorName.Text = ""
            tbContactPerson.Text = ""
            tbContactTitle.Text = ""
            tbContactPh.Text = ""
            btnSaveHd.Visible = True
            btnCancelHd.Visible = True
            btnReset.Visible = True
        Catch ex As Exception
            Throw New Exception("Clear Hd Error " + ex.ToString)
        End Try
    End Sub
    Private Sub cleardt()
        Try
            lbItemNo.Text = ""
            tbContactName.Text = ""
            tbNoKTP.Text = ""
            tbTitle.Text = ""
            tbContactAddr1.Text = ""
            tbContactAddr2.Text = ""
            tbPostalCode.Text = ""
            ddlCountry.SelectedIndex = 0
            tbTelephone.Text = ""
            tbContactFax.Text = ""
            tbContactEmail.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub
    Private Sub cleardt2()
        Try
            tbDeliveryCode.Text = ""
            tbDeliveryPlace.Text = ""
            tbAddress1Addr.Text = ""
            tbAddress2Addr.Text = ""
            tbZipCodeAddr.Text = ""
            TbPhoneAddr.Text = ""
            tbFaxAddr.Text = ""
            tbEmailAddr.Text = ""
            tbPersonAddr.Text = ""
            tbTitleAddr.Text = ""
            tbHpAddr.Text = ""
            tbRemarkAddr.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt2 Error " + ex.ToString)
        End Try
    End Sub

    Private Sub cleardt3()
        Try
            lbItemBank.Text = ""
            ddlBank.SelectedValue = ""
            tbAccounNo.Text = ""
            tbAccountName.Text = ""
            tbSwiftCode.Text = ""
            tbBranch.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt3 Error " + ex.ToString)
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
            lbStatus.Text = "btn Expand Error : " + ex.ToString
        End Try
    End Sub

    Function CekHd() As Boolean
        Try
            If tbCode.Text.Trim = "" Then
                lbStatus.Text = "Supplier Code must have value"
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim = "" Then
                lbStatus.Text = "Supplier Name must have value"
                tbName.Focus()
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
            If Dr("ContactName").ToString.Trim = "" Then
                lbStatus.Text = "Contact Name Must Have Value"
                tbContactName.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Cek Dt Error : " + ex.ToString)
        End Try
    End Function

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
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
                    PnlAddress.Visible = True
                    pnlDt.Visible = True
                    btnAddDt.Visible = True
                    PnlBank.Visible = True
                    PnlEditBank.Visible = False
                    ViewState("SuppCode") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    BindDataDt(ViewState("SuppCode"))
                    GridViewAddr.PageIndex = 0
                    BindDataDt2(ViewState("SuppCode"))
                    GridDt3.PageIndex = 0
                    BindDataDt3(ViewState("SuppCode"))
                    FillTextBoxHd(ViewState("SuppCode"))
                    ViewState("StateHd") = "View"
                    ModifyInput(False)
                    Menu1.Visible = True
                    Menu1.Items.Item(0).Selected = True
                    MultiView1.Visible = True
                    MultiView1.ActiveViewIndex = 0
                    btnSaveHd.Visible = False
                    btnCancelHd.Visible = False
                    btnReset.Visible = False
                ElseIf DDL.SelectedValue = "Edit" Then
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
                    PnlHd.Visible = False
                    pnlView.Visible = True
                    pnlDt.Visible = False
                    pnlEditDt.Visible = False
                    PnlAddress.Visible = False
                    PnlEditAddress.Visible = False
                    PnlBank.Visible = False
                    PnlEditBank.Visible = False
                    PnlAddress.Visible = False
                    ViewState("SuppCode") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    BindDataDt(ViewState("SuppCode"))
                    FillTextBoxHd(ViewState("SuppCode"))
                    tbNPWP.Enabled = ddlPKP.SelectedIndex = 0
                    ViewState("StateHd") = "Edit"
                    ModifyInput(True)
                    EnableHd(GridDt.Rows.Count = 0)
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
                        ViewState("SuppCode") = GVR.Cells(2).Text
                        DeleteHd()
                        BindData(1)
                    Catch ex As Exception
                        lbStatus.Text = "DDL.SelectedValue = Delete Error : " + ex.ToString
                    End Try
                ElseIf DDL.SelectedValue = "Print" Then
                    'Dim ReportGw As New ReportDocument
                    'Dim Reportds As DataSet
                    'Reportds = SQLExecuteQuery("EXEC S_GLFormJE " + QuotedStr(GVR.Cells(2).Text))
                    'ReportGw.Load(Server.MapPath("~\Rpt\FormJEntry.Rpt"))
                    'ReportGw.SetDataSource(Reportds.Tables(0))
                    'Session("Report") = ReportGw
                    'Response.Write("<script language='javascript'> { window.open(""../../Rpt/PrintMaster.Aspx"", ""List"",""scrollbars=yes, resizable=yes,width=600,height=500"");}</script>")
                End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Try
            ViewState("SortExpression") = e.SortExpression
            BindData(2)
        Catch ex As Exception
            lbStatus.Text = "Grid View 1 Sorting Error : " + ex.ToString
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
        Dim dr() As DataRow
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text + " AND SuppCode = " + QuotedStr(tbCode.Text))
        dr(0).Delete()

        SQLExecuteNonQuery("DELETE FROM MsSuppContact WHERE ItemNo = " + GVR.Cells(1).Text + " AND SuppCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)
        
        GridDt.DataSource = ViewState("Dt")
        GridDt.DataBind()
        lbStatus.Text = "Data Deleted"
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
            tbContactName.Text = GVR.Cells(2).Text.Replace("&nbsp;", "")
            tbTitle.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbContactAddr1.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbContactAddr2.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbPostalCode.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            tbTelephone.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            tbContactFax.Text = GVR.Cells(9).Text.Replace("&nbsp;", "")
            tbContactEmail.Text = GVR.Cells(10).Text.Replace("&nbsp;", "")
            tbNoKTP.Text = GVR.Cells(11).Text.Replace("&nbsp;", "")
            pnlEditDt.Visible = True
            pnlDt.Visible = False
            ViewState("StateDt") = "Edit"
            btnSave.Focus()
            tbContactAddr1.Enabled = tbContactTitle.Text <> "N"
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTerm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTerm.TextChanged
        Dim DsTerm As DataSet
        Dim DrTerm As DataRow
        Try
            DsTerm = SQLExecuteQuery("Select TermCode, TermName from MsTerm where TermCode = '" + tbTerm.Text + "'", ViewState("DBConnection").ToString)
            If DsTerm.Tables(0).Rows.Count = 1 Then
                DrTerm = DsTerm.Tables(0).Rows(0)
                tbTerm.Text = DrTerm("TermCode").ToString
                ddlTerm.SelectedValue = DrTerm("TermCode").ToString
            Else
                tbTerm.Text = ""
                ddlTerm.Text = ""
            End If
            ddlPKP.Focus()

        Catch ex As Exception
            lbStatus.Text = "ddl Term Error : " + ex.ToString
        End Try
    End Sub


    Protected Sub tbCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbCode.TextChanged
       
        Try

            If Left(tbCode.Text, 4) <> "SUP/" Then
                tbCode.Text = "SUP/" + tbCode.Text
            Else
                tbCode.Text = tbCode.Text
            End If

        Catch ex As Exception
            lbStatus.Text = "tbCode : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSaveHd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveHd.Click
        Try
            SaveHd()
            
        Catch ex As Exception
            lbStatus.Text = "Save Hd Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelHd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelHd.Click
        Try
            pnlView.Visible = False
            PnlHd.Visible = True
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            If tbCode.Enabled Then
                tbCode.Text = ""
            End If
            ClearHd()
        Catch ex As Exception
            lbStatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTerm.SelectedIndexChanged
        Dim DsTerm As DataSet
        Dim DrTerm As DataRow
        Try
            DsTerm = SQLExecuteQuery("Select TermCode, TermName from MsTerm where TermCode= '" + ddlTerm.SelectedValue + "'", ViewState("DBConnection").ToString)
            If DsTerm.Tables(0).Rows.Count = 1 Then
                DrTerm = DsTerm.Tables(0).Rows(0)
                tbTerm.Text = DrTerm("TermCode").ToString
                ddlTerm.SelectedValue = DrTerm("TermCode").ToString
            Else
                tbTerm.Text = ""
                ddlTerm.Text = ""
            End If
            ddlPKP.Focus()

        Catch ex As Exception
            lbStatus.Text = "ddl Term Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlPKP_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPKP.SelectedIndexChanged
        tbNPWP.Enabled = ddlPKP.SelectedIndex = 0
        If ddlPKP.SelectedIndex = 0 Then
            tbNPWP.Text = ""
            tbNPWP.Focus()
        Else : tbNPWP.Text = ""
            ddlPPN.Focus()
        End If
        Exit Sub
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt.Click
        If CheckMenuLevel("Insert") = False Then
            Exit Sub
        End If
        cleardt()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = "Date Must Filled"
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
        tbContactName.Focus()
        Exit Sub
    End Sub
    Private Sub SaveDt1()
        Dim SQLString As String
        Try
            If tbContactName.Text.Trim = "" Then
                lbStatus.Text = "<script language='javascript'> {alert(Contact Name Must Have Value);}</script>"
                tbContactName.Focus()
                Exit Sub
            End If
            
            If ViewState("StateDt") = "Insert" Then
                SQLString = "INSERT INTO MsSuppContact (suppcode, ItemNo, ContactName, ContactTitle, Address1, Address2, Country, ZipCode, Phone, Fax, Email,NoKtp, userid, userdate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(lbItemNo.Text) + ", " + QuotedStr(tbContactName.Text) + ", " + QuotedStr(tbTitle.Text) + ", " + QuotedStr(tbContactAddr1.Text) + ", " + QuotedStr(tbContactAddr2.Text) + ", " + _
                QuotedStr(ddlCountry.SelectedValue) + " , " + QuotedStr(tbPostalCode.Text) + ", " + QuotedStr(tbTelephone.Text) + ", " + QuotedStr(tbContactFax.Text) + ", " + QuotedStr(tbContactEmail.Text) + ", " + QuotedStr(tbNoKTP.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                SQLString = "UPDATE MsSuppContact SET ContactName = " + QuotedStr(tbContactName.Text) + ", ContactTitle = " + QuotedStr(tbTitle.Text) + ", " + _
                " Address1 = " + QuotedStr(tbContactAddr1.Text) + ", Address2= " + QuotedStr(tbContactAddr2.Text) + ", " + _
                " Country = " + QuotedStr(ddlCountry.SelectedValue) + ", ZipCode = " + QuotedStr(tbPostalCode.Text) + ", " + _
                " Phone = " + QuotedStr(tbTelephone.Text) + ", Fax = " + QuotedStr(tbContactFax.Text) + ", " + _
                " email = " + QuotedStr(tbContactEmail.Text) + ",NoKtp = " + QuotedStr(tbNoKTP.Text) + ", UserDate = getDate()" + _
                " WHERE SuppCode = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNo.Text)
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            
            pnlEditDt.Visible = False
            pnlDt.Visible = True
            EnableHd(GridDt.Rows.Count = 0)
            BindDataDt(ViewState("SuppCode"))
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Dt1 Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub SaveDt2()
        Dim SQLString As String
        Try
            If tbDeliveryCode.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Delivery Code must have value")
                tbDeliveryCode.Focus()
                Exit Sub
            End If
            If tbDeliveryPlace.Text.Trim = "" Then
                lbStatus.Text = MessageDlg("Delivery Place must have value")
                tbDeliveryPlace.Focus()
                Exit Sub
            End If
            If tbAddress1Addr.Text.Trim = "" Then
                lbStatus.Text = "<script language='javascript'> {alert(Address Must Have Value);}</script>"
                tbAddress1Addr.Focus()
                Exit Sub
            End If

            Dim dt As DataTable
            SQLString = "SELECT SuppCode, DeliveryCode FROM MsSuppAddress WHERE SuppCode = " + QuotedStr(tbCode.Text) + " AND DeliveryCode = " + QuotedStr(tbDeliveryCode.Text)
            dt = SQLExecuteQuery(SQLString, ViewState("DBConnection").ToString).Tables(0)

            If ViewState("StateDt2") = "Insert" Then
                If dt.Rows.Count <> 0 Then
                    lbStatus.Text = MessageDlg("Delivery Code already exists")
                    tbDeliveryCode.Focus()
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsSuppAddress (SuppCode, DeliveryCode, DeliveryPlace, Address1, Address2, ZipCode, Phone, Fax, Email, ContactPerson, ContactTitle, ContactHP, Remark, UserId, UserDate) " + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbDeliveryCode.Text) + ", " + QuotedStr(tbDeliveryPlace.Text) + ", " + QuotedStr(tbAddress1Addr.Text) + ", " + QuotedStr(tbAddress2Addr.Text) + ", " + QuotedStr(tbZipCodeAddr.Text) + ", " + QuotedStr(TbPhoneAddr.Text) + ", " + _
               QuotedStr(tbFaxAddr.Text) + " , " + QuotedStr(tbEmailAddr.Text) + ", " + QuotedStr(tbPersonAddr.Text) + ", " + QuotedStr(tbTitleAddr.Text) + ", " + QuotedStr(tbHpAddr.Text) + ", " + _
               QuotedStr(tbRemarkAddr.Text) + " , " + QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                SQLString = "UPDATE MsSuppAddress SET Address1 = " + QuotedStr(tbAddress1Addr.Text) + ", Address2 = " + QuotedStr(tbAddress2Addr.Text) + ", " + _
                " ZipCode = " + QuotedStr(tbZipCodeAddr.Text) + ", Phone= " + QuotedStr(TbPhoneAddr.Text) + ", " + _
                " Fax = " + QuotedStr(tbFaxAddr.Text) + ", Email = " + QuotedStr(tbEmailAddr.Text) + ", " + _
                " ContactPerson = " + QuotedStr(tbPersonAddr.Text) + ", ContactTitle = " + QuotedStr(tbTitleAddr.Text) + ", " + _
                " ContactHP = " + QuotedStr(tbHpAddr.Text) + ", Remark = " + QuotedStr(tbRemarkAddr.Text) + ", UserDate = getDate(), " + _
                " DeliveryPlace = " + QuotedStr(tbDeliveryPlace.Text) + _
                " WHERE SuppCode = " + QuotedStr(tbCode.Text) + " AND DeliveryCode = " + QuotedStr(tbDeliveryCode.Text) + " "
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditAddress.Visible = False
            PnlAddress.Visible = True
        Catch ex As Exception
            Throw New Exception("Save Dt2 Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddDt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddDt2.Click
        cleardt2()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = "Date Must Filled"
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
            Row = ViewState("Dt2").select("")
            i = Row.Length
        End If

        ViewState("StateDt2") = "Insert"
        tbDeliveryCode.Enabled = True
        PnlEditAddress.Visible = True
        PnlAddress.Visible = False
        EnableHd(False)
        tbAddress1Addr.Focus()
        Exit Sub
    End Sub

    Protected Sub btnSaveAddr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAddr.Click
        Try
            SaveDt2()
            'PnlEditAddress.Visible = False
            'PnlAddress.Visible = True
            GridViewAddr.PageIndex = 0
            BindDataDt2(ViewState("SuppCode"))
        Catch ex As Exception
            lbStatus.Text = "Save Addr Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelAddr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelAddr.Click
        Try
            PnlEditAddress.Visible = False
            PnlAddress.Visible = True
            EnableHd(GridDt.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Addr Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewAddr_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewAddr.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridViewAddr.Rows(e.NewEditIndex)

            tbDeliveryCode.Text = GVR.Cells(1).Text.Replace("&nbsp;", "")
            tbDeliveryPlace.Text = GVR.Cells(2).Text.Replace("&nbsp;", "")
            tbAddress1Addr.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbAddress2Addr.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbZipCodeAddr.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            TbPhoneAddr.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            tbFaxAddr.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            tbEmailAddr.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            tbPersonAddr.Text = GVR.Cells(9).Text.Replace("&nbsp;", "")
            tbTitleAddr.Text = GVR.Cells(10).Text.Replace("&nbsp;", "")
            tbHpAddr.Text = GVR.Cells(11).Text.Replace("&nbsp;", "")
            tbRemarkAddr.Text = GVR.Cells(12).Text.Replace("&nbsp;", "")
            tbDeliveryCode.Enabled = False
            PnlEditAddress.Visible = True
            PnlAddress.Visible = False
            EnableHd(False)
            ViewState("StateDt2") = "Edit"
            tbAddress1Addr.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid View Addr Row Editing Error : " + ex.ToString
        End Try

    End Sub

    Protected Sub GridViewAddr_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewAddr.RowDeleting
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim dr() As DataRow
        Dim GVR As GridViewRow = GridViewAddr.Rows(e.RowIndex)
        dr = ViewState("Dt2").Select("DeliveryCode = '" + GVR.Cells(1).Text + "' AND SuppCode = " + QuotedStr(tbCode.Text))
        dr(0).Delete()

        SQLExecuteNonQuery("DELETE FROM MsSuppAddress WHERE DeliveryCode = '" + GVR.Cells(1).Text + "' AND SuppCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

        GridViewAddr.DataSource = ViewState("Dt2")
        GridViewAddr.DataBind()
        lbStatus.Text = "Data Deleted"
        EnableHd(GridViewAddr.Rows.Count = 0)
    End Sub

    Protected Sub GridViewAddr_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewAddr.RowCommand
        Try
            If e.CommandName = "Insert" Then

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid View Addr Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewAddr_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewAddr.PageIndexChanging
        Try
            GridViewAddr.PageIndex = e.NewPageIndex
            GridViewAddr.DataSource = ViewState("Dt2")
            GridViewAddr.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT2 Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridDt3.PageIndexChanging
        Try
            GridDt3.PageIndex = e.NewPageIndex
            GridDt3.DataSource = ViewState("Dt3")
            GridDt3.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid dT3 Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridDt3.RowCommand
        Try
            If e.CommandName = "Insert" Then

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid Dt 3 Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridDt3_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridDt3.RowDeleting
        Dim dr() As DataRow
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridDt3.Rows(e.RowIndex)
        dr = ViewState("Dt3").Select("Item = " + GVR.Cells(1).Text + " AND SuppCode = " + QuotedStr(tbCode.Text))
        dr(0).Delete()

        SQLExecuteNonQuery("DELETE FROM MsSuppBank WHERE Item = " + GVR.Cells(1).Text + " AND SuppCode = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString)

        GridDt3.DataSource = ViewState("Dt3")
        GridDt3.DataBind()
        lbStatus.Text = "Data Deleted"
        EnableHd(GridDt3.Rows.Count = 0)
    End Sub

    Protected Sub GridDt3_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridDt3.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridDt3.Rows(e.NewEditIndex)

            lbItemBank.Text = GVR.Cells(1).Text
            ddlBank.SelectedValue = GVR.Cells(2).Text
            tbAccounNo.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbAccountName.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbSwiftCode.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbBranch.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            PnlEditBank.Visible = True
            PnlBank.Visible = False
            EnableHd(False)
            ViewState("StateDt3") = "Edit"
            ddlBank.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt3 Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDt3.Click
        Try
            PnlEditBank.Visible = False
            PnlBank.Visible = True
            EnableHd(GridDt3.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Dt3 Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveDt3()
        Dim SQLString As String
        Try
            If ddlBank.Text.Trim = "" Then
                lbStatus.Text = "<script language='javascript'> {alert(Bank Must Have Value);}</script>"
                ddlBank.Focus()
                Exit Sub
            End If
            If ViewState("StateDt3") = "Insert" Then
                SQLString = "INSERT INTO MsSuppBank (SuppCode, Item, Bank, AccountName, AccountNo, SwiftCode, Branch, UserId, UserDate) " + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemBank.Text + ", " + QuotedStr(ddlBank.SelectedValue) + ", " + QuotedStr(tbAccountName.Text) + ", " + QuotedStr(tbAccounNo.Text) + ", " + _
               QuotedStr(tbSwiftCode.Text) + ", " + QuotedStr(tbBranch.Text) + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                SQLString = "UPDATE MsSuppBank SET Bank = " + QuotedStr(ddlBank.SelectedValue) + ", AccountName = " + QuotedStr(tbAccountName.Text) + ", " + _
                " AccountNo = " + QuotedStr(tbAccounNo.Text) + ", SwiftCode= " + QuotedStr(tbSwiftCode.Text) + ", " + _
                " Branch = " + QuotedStr(tbBranch.Text) + ", UserDate = getDate()" + _
                " WHERE SuppCode = " + QuotedStr(tbCode.Text) + " AND Item = " + lbItemBank.Text + " "
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditBank.Visible = False
            PnlBank.Visible = True
            GridDt3.PageIndex = 0
            BindDataDt3(ViewState("SuppCode"))
        Catch ex As Exception
            Throw New Exception("Save Dt3 Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveDt3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDt3.Click
        Try
            SaveDt3()

        Catch ex As Exception
            lbStatus.Text = "Save Dt3 Error : " + ex.ToString
        End Try
    End Sub
    
    Protected Sub btnAddBank_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBank.Click
        cleardt3()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = "Date Must Filled"
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
            Row = ViewState("Dt3").select("")
            i = Row.Length
        End If

        ViewState("StateDt3") = "Insert"
        If i > 0 Then
            lbItemBank.Text = (CInt(Row(i - 1)("Item").ToString) + 1).ToString
        Else
            lbItemBank.Text = "1"
        End If
        PnlEditBank.Visible = True
        PnlBank.Visible = False
        EnableHd(False)
        ddlBank.Focus()
        Exit Sub
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        Dim paramgo As String
        'Make the selected menu item reflect the correct imageurl

        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
                'Menu1.Items(i).ImageUrl = "selectedtab.gif"
            ElseIf Menu1.Items(3).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                    GVR = GridView1.Rows(CInt(e.CommandArgument))
                    paramgo = tbCode.Text + "|" + tbName.Text
                    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Supplier', '" + Request.QueryString("KeyId") + "','" + paramgo + "','AssSuppProduct');", True)
                    End If
                Catch ex As Exception
                    lbStatus.Text = "DDL.SelectedValue = Product Error : " + ex.ToString
                End Try
            End If
        Next

    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Supplier_Code = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)
            tbCode.Text = Nmbr
            BindToText(tbName, Dt.Rows(0)("Supplier_Name").ToString)
            BindToDropList(ddlSuppClass, Dt.Rows(0)("Supplier_Class").ToString)
            BindToDropList(ddlSuppType, Dt.Rows(0)("Supplier_Type").ToString)
            BindToDropList(ddlPPN, Dt.Rows(0)("PPN").ToString)
            BindToText(tbAddress1, Dt.Rows(0)("Address_1").ToString)
            BindToText(tbAddress2, Dt.Rows(0)("Address_2").ToString)
            BindToDropList(ddlCity, Dt.Rows(0)("City").ToString)
            BindToText(tbPostCode, Dt.Rows(0)("Zip_Code").ToString)
            BindToText(tbPhone, Dt.Rows(0)("Phone").ToString)
            BindToText(tbFax, Dt.Rows(0)("Fax").ToString)
            BindToText(tbEmail, Dt.Rows(0)("Email").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToText(tbTerm, Dt.Rows(0)("Term").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToText(tbNPWP, Dt.Rows(0)("NPWP").ToString)
            BindToDropList(ddlAktive, Dt.Rows(0)("FgActive").ToString)
            BindToDropList(ddlPKP, Dt.Rows(0)("NPPKP").ToString)
            BindToText(tbDirectorName, Dt.Rows(0)("Ownername").ToString)
            BindToText(tbContactTitle, Dt.Rows(0)("Contact_Title").ToString)
            BindToText(tbContactPerson, Dt.Rows(0)("Contact_Person").ToString)
            BindToText(tbContactPh, Dt.Rows(0)("Contact_Hp").ToString)
            BindToText(tbContactEmail, Dt.Rows(0)("Email").ToString)
            BindToText(tbMemberOfGroup, Dt.Rows(0)("MemberOfGroup").ToString)
            BindToText(tbSisterCompany, Dt.Rows(0)("SisterCompany").ToString)
            BindToText(tbISOCertNo, Dt.Rows(0)("ISOCertNo").ToString)
            BindToText(tbSIUPNo, Dt.Rows(0)("SIUPNo").ToString)
            BindToText(tbNoKTPHd, Dt.Rows(0)("NoKTP").ToString)
            BindToDropList(ddlFgSubkon, Dt.Rows(0)("FgSubkon").ToString)
            BindToDropList(ddlWrhsSubkon, Dt.Rows(0)("WrhsSubkon").ToString)


            ddlWrhsSubkon.Enabled = ddlFgSubkon.SelectedValue = "Y" And ViewState("StateHd") <> "View"
        Catch ex As Exception
            Throw New Exception("fill text box header error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub ddlFgSubkon_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFgSubkon.SelectedIndexChanged
        ddlWrhsSubkon.Enabled = ddlFgSubkon.SelectedIndex = 0
        If ddlFgSubkon.SelectedIndex = 0 Then
            ddlWrhsSubkon.SelectedValue = ""
            ddlWrhsSubkon.Enabled = True
        Else
            ddlWrhsSubkon.SelectedValue = ""
            ddlWrhsSubkon.Enabled = False
        End If
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'Dim Result As String
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_MKRptSupplierList '','','', '1',1, 1, '', '', " + QuotedStr(ViewState("UserId").ToString)
            Session("ReportFile") = Server.MapPath("~\Rpt\RptSuppList.frx")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub SetInit()
        Me.tbPostCode.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbPhone.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbFax.Attributes.Add("OnKeyDown", "return PressNumeric();")
        Me.tbContactPh.Attributes.Add("OnKeyDown", "return PressNumeric();")
    End Sub
End Class
