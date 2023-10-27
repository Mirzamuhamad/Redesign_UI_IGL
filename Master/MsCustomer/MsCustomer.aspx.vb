Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web.Design
Imports System.IO

Partial Class Master_MsCustomer_MsCustomer
    Inherits System.Web.UI.Page
    Protected con As New SqlConnection
    Protected da, da1 As New SqlDataAdapter
    Protected GetStringHd As String = "SELECT * From VMsCustomer"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                FillCombo(ddlCustType, "SELECT CustTypeCode, CustTypeName From MsCustType", True, "CustTypeCode", "CustTypeName", ViewState("DBConnection").ToString)
                FillCombo(ddlCustGroup, "Select CustGroupCode, CustGroupName From MsCustGroup", True, "CustGroupCode", "CustGroupName", ViewState("DBConnection").ToString)
                FillCombo(ddlCurr, "SELECT CurrCode From MsCurrency", True, "CurrCode", "CurrCode", ViewState("DBConnection").ToString)
                FillCombo(ddlTerm, "SELECT TermCode, TermName From MsTerm", True, "TermCode", "TermName", ViewState("DBConnection").ToString)
                FillCombo(ddlPaymentTo, "SELECT PayCode, PayName From MsPayType", True, "PayCode", "PayName", ViewState("DBConnection").ToString)
                FillCombo(ddlCity, "SELECT CityCode, CityName From MsCity", True, "CityCode", "CityName", ViewState("DBConnection").ToString)
                FillCombo(ddlCityAddr, "SELECT CityCode, CityName From MsCity", True, "CityCode", "CityName", ViewState("DBConnection").ToString)
                FillCombo(ddlCustField, "SELECT custfieldcode, custfieldname From Mscustfield", True, "custfieldCode", "custfieldName", ViewState("DBConnection").ToString)
                FillCombo(ddlKodePPn, "SELECT PPnCode, PPnName From MsKodePPn", True, "PPnCode", "PPnName", ViewState("DBConnection").ToString)
                ViewState("MenuLevel") = SetMenuLevel(Request.QueryString("ContainerId").ToString, ViewState("UserId").ToString, ViewState("DBConnection").ToString)
                btnAdd.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnadd2.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnadddt.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddBillTo.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddField.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddAddress.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnAddTax.Visible = ViewState("MenuLevel").Rows(0)("FgInsert") = "Y"
                btnprint.Visible = ViewState("MenuLevel").rows(0)("FgPrint") = "Y"
            End If
            lbStatus.Text = ""

            If Not Session("StrResult") Is Nothing Then
                BindData(2, Session("StrResult"))
                Session("StrResult") = Nothing
            End If

            If ViewState("Sender") = "btnBillTo" Then
                tbBillTo.Text = Session("Result")(0).ToString
                tbBillToName.Text = Session("Result")(1).ToString
                'btnSaveBillTo.Focus()
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
                            dr("ContactType") = drResult("ContactType")
                            dr("ContactName") = drResult("ContactName")
                            'dr("Concat") = DBNull.Value
                            dr("ContactTitle") = drResult("ContactTitle")
                            dr("Address1") = drResult("DeliveryAddr1")
                            dr("Address2") = drResult("DeliveryAddr2")
                            dr("ZipCode") = drResult("ZipCode")
                            dr("Phone") = drResult("Phone")
                            dr("Fax") = drResult("Fax")
                            dr("Email") = drResult("Email")
                            ViewState("Dt").Rows.Add(dr)
                        Else
                            'edit
                            Row(0).BeginEdit()
                            Row(0)("ContactType") = drResult("ContactType")
                            Row(0)("ContactName") = drResult("ContactName")
                            Row(0)("ContactTitle") = drResult("ContactTitle")
                            'Row(0)("City") = DBNull.Value
                            Row(0)("Address1") = drResult("Address1")
                            Row(0)("Address2") = drResult("Address2")
                            Row(0)("ZipCode") = drResult("ZipCode")
                            Row(0)("Phone") = drResult("Phone")
                            Row(0)("Fax") = drResult("Fax")
                            Row(0)("Email") = drResult("Email")
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
            'PnlLimit.Visible = False
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
            DS = SQLExecuteQuery("SELECT A.*, Q.PPnName, B.CustGroupName, C.CustTypeName, D.CityName, E.TermName,P.PayCode,P.PayName FROM MsCustomer A LEFT OUTER JOIN MsCustGroup B ON A.CustGroup = B.CustGroupCode LEFT OUTER JOIN MsCustType C ON A.CustType = C.CustTypeCode LEFT OUTER JOIN MsCity D ON A.City = D.CityCode LEFT OUTER JOIN MsTerm E ON A.Term = E.TermCode LEFT OUTER JOIN MsPayType P ON A.PaymentTo = P.PayCode LEFT OUTER JOIN MsKodePPn Q ON A.KodePPn = Q.PPnCode " + StrFilter, ViewState("DBConnection").ToString)

            If DS.Tables(0).Rows.Count = 0 Then
                lbStatus.Text = "No Data"
                pnlNav.Visible = False
            End If

            DV = DS.Tables(0).DefaultView
            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "CustCode ASC"
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
            tbName.Enabled = State
            tbMember.Enabled = State
            tbSister.Enabled = State
            ddlPaymentTo.Enabled = State
            ddlCustGroup.Enabled = State
            ddlCustType.Enabled = State
            ddlCity.Enabled = State
            tbAddress1.Enabled = State
            tbAddress2.Enabled = State
            ddlCurr.Enabled = State
            tbTerm.Enabled = State
            ddlTerm.Enabled = State
            tbAddress1.Enabled = State
            tbAddress2.Enabled = State
            tbPhone.Enabled = State
            ddlPriceBySO.Enabled = State
            ddlCity.Enabled = State
            tbPostCode.Enabled = State
            tbFax.Enabled = State
            tbEmail.Enabled = State
            tbFPCustKode.Enabled = State
            tbNPWP.Enabled = State
            ddlPPN.Enabled = State
            ddlKodePPn.Enabled = State
            ddlFgLimit.Enabled = State
            ddlAktive.Enabled = State
            tbContactTitle.Enabled = State
            tbContactHp.Enabled = State
            tbCName.Enabled = State
            'PnlLimit.Visible = False
            btnSaveHd.Enabled = State
            btnCancelHd.Enabled = State
            tbCEmail.Enabled = State
        Catch ex As Exception
            Throw New Exception("Modify Input Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableHd(ByVal State As Boolean)
        Try
            'BtnGetDt.Enabled = State
            tbCode.Enabled = State
            'tbName.Enabled = State
            'ddlCustGroup.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Sub EnableAddress(ByVal State As Boolean)
        Try
            tbDeliveryCode.Enabled = State
            tbDeliveryName.Enabled = State
            ddlDeliveryType.Enabled = State
            tbAddress1Addr.Enabled = State
            tbAddress2Addr.Enabled = State
            ddlCityAddr.Enabled = State
            tbZipCodeAddr.Enabled = State
            TbPhoneAddr.Enabled = State
            tbFaxAddr.Enabled = State
            tbContactPersonAddr.Enabled = State
        Catch ex As Exception
            Throw New Exception("Enable Hd Error " + ex.ToString)
        End Try
    End Sub

    Private Function GetString(ByVal Nmbr As String) As String
        Return "SELECT * FROM msCustomer  WHERE CustCode = '" + Nmbr + "'"
    End Function


    Private Function GetStringDtBillTo(ByVal Nmbr As String) As String
        Return "SELECT A.CustCode, A.CustCollect, B.CustName, A.UserId, A.UserDate FROM mscustbillto A INNER JOIN MsCustomer B ON A.CustCollect = B.CustCode WHERE A.CustCode = '" + Nmbr + "'"
    End Function

    Private Function GetStringDtField(ByVal Nmbr As String) As String
        Return "SELECT A.CustCode, A.CustField, B. custfieldName, A.UserId, A.UserDate FROM mscustinfofield A INNER JOIN  mscustfield B ON A.CustField = B.custfieldcode WHERE A.CustCode = '" + Nmbr + "'"
    End Function

    Private Function GetStringDt(ByVal Nmbr As String) As String
        Return "SELECT CustCode, ItemNo, ContactType, ContactName, ContactTitle, Address1, Address2, ZipCode, Phone, Fax, Email, UserID, UserDate FROM msCustcontact WHERE CustCode = '" + Nmbr + "'"
    End Function

    Private Function GetStringDtAddr(ByVal Nmbr As String) As String
        Return "SELECT CustCode, DeliveryCode, DeliveryName, DeliveryType, DeliveryAddr1, DeliveryAddr2, City, ZipCode, PhoneNo, Fax, ContactPerson, UserId, UserId FROM mscustaddress WHERE CustCode = '" + Nmbr + "'"
    End Function


    Private Function GetStringDtTax(ByVal Nmbr As String) As String
        Return "SELECT A.CustCode, A.CustTaxAddress, A.CustTaxNPWP, B.CustName, A.UserId, A.UserDate FROM mscustTaxAddress A INNER JOIN MsCustomer B ON A.CustCode = B.CustCode WHERE A.CustCode = '" + Nmbr + "'"
    End Function

    Private Sub BindDataDt(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("Dt") = Nothing
            dt = SQLExecuteQuery(GetStringDt(Referens), ViewState("DBConnection")).Tables(0)
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
            PnlAddress.Visible = False
            PnlBillTo.Visible = False
            PnlField.Visible = False
            pnlTax.Visible = False
            btnAddDt.Visible = False
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
        Dim SQLString As String
        Try
            If tbCode.Text.Trim = "" Then
                lbStatus.Text = "Code Must Have Value"
                tbCode.Focus()
                Exit Sub
            End If
            If tbName.Text.Trim = "" Then
                lbStatus.Text = "Name Must Have Value"
                tbName.Focus()
                Exit Sub
            End If
            If tbMember.Text.Trim = "" Then
                lbStatus.Text = "Member Of Group Must Have Value"
                tbMember.Focus()
                Exit Sub
            End If
            If tbSister.Text.Trim = "" Then
                lbStatus.Text = "Sister Company Must Have Value"
                tbSister.Focus()
                Exit Sub
            End If
            If ddlCustGroup.Text.Trim = "" Then
                lbStatus.Text = "Customer Group Must Have Value"
                ddlCustGroup.Focus()
                Exit Sub
            End If

            If ddlCustType.Text.Trim = "" Then
                lbStatus.Text = "Customer Type Must Have Value"
                ddlCustType.Focus()
                Exit Sub
            End If

            If ddlCurr.Text.Trim = "" Then
                lbStatus.Text = "Currency Must Have Value"
                ddlCurr.Focus()
                Exit Sub
            End If

            If ddlPaymentTo.Text.Trim = "" Then
                lbStatus.Text = "Payment To Must Have Value"
                ddlPaymentTo.Focus()
                Exit Sub
            End If

            If ddlTerm.Text.Trim = "" Then
                lbStatus.Text = "Payment Term Must Have Value"
                tbTerm.Focus()
                Exit Sub
            End If


            If ViewState("StateHd") = "Insert" Then
                If SQLExecuteScalar("SELECT Customer_Code From VMsCustomer WHERE Customer_Code = " + QuotedStr(tbCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "Customer " + QuotedStr(tbCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsCustomer (CustCode, CustName,MemberOfGroup,SisterCompany,CustGroup, CustType, Address1, Address2, City, ZipCode, Phone, Fax, Email, " + _
               "CurrCode, PaymentTo,Term, NPWP,FPCustKode, FgPPN, FgLimit, ContactName, ContactTitle, " + _
               "ContactHP, ContactEmail, FgBroker, FgActive,PriceBySO, KodePPn, Nik, Tempat, TglLahir, WargaNegara, UserID, UserDate) " + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbName.Text) + "," + QuotedStr(tbMember.Text) + "," + QuotedStr(tbSister.Text) + "," + QuotedStr(ddlCustGroup.SelectedValue) + ", " + QuotedStr(ddlCustType.SelectedValue) + ", " + QuotedStr(tbAddress1.Text) + ", " + QuotedStr(tbAddress2.Text) + ", " + _
               QuotedStr(ddlCity.SelectedValue) + ", " + QuotedStr(tbPostCode.Text) + ", " + QuotedStr(tbPhone.Text) + ", " + QuotedStr(tbFax.Text) + ", " + QuotedStr(tbEmail.Text) + ", " + _
               QuotedStr(ddlCurr.SelectedValue) + ", " + QuotedStr(ddlPaymentTo.SelectedValue) + ", " + QuotedStr(ddlTerm.SelectedValue) + ", " + QuotedStr(tbNPWP.Text) + ", " + QuotedStr(tbFPCustKode.Text) + "," + QuotedStr(ddlPPN.SelectedValue) + ", " + QuotedStr(ddlFgLimit.SelectedValue) + ", " + _
               QuotedStr(tbCName.Text) + ", " + QuotedStr(tbContactTitle.Text) + ", " + _
               QuotedStr(tbContactHp.Text) + ", " + QuotedStr(tbCEmail.Text) + ", 'N' " + ", " + QuotedStr(ddlAktive.SelectedValue) + "," + QuotedStr(ddlAktive.SelectedValue) + "," + QuotedStr(ddlKodePPn.SelectedValue) + "," + _
               QuotedStr(tbNik.Text) + ", " + QuotedStr(tbTempat.Text) + ", '" + Format(tbDateLahir.SelectedValue, "yyyy-MM-dd") + "' ," + QuotedStr(ddlWargaNegara.SelectedValue) + "," + _
               QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                SQLString = "UPDATE MsCustomer SET CustGroup = " + QuotedStr(ddlCustGroup.SelectedValue) + ", " + _
                " MemberOfGroup = " + QuotedStr(tbMember.Text) + ", SisterCompany= " + QuotedStr(tbSister.Text) + ", " + _
                " CustType = " + QuotedStr(ddlCustType.SelectedValue) + ", address1= " + QuotedStr(tbAddress1.Text) + ", " + _
                " address2 = " + QuotedStr(tbAddress2.Text) + ", city = " + QuotedStr(ddlCity.SelectedValue) + ", " + _
                " zipcode = " + QuotedStr(tbPostCode.Text) + ", phone = " + QuotedStr(tbPhone.Text) + ", " + _
                " Fax = " + QuotedStr(tbFax.Text) + ", email = " + QuotedStr(tbEmail.Text) + ", " + _
                " currcode = " + QuotedStr(ddlCurr.SelectedValue) + ", term = " + QuotedStr(ddlTerm.SelectedValue) + ", " + _
                " npwp = " + QuotedStr(tbNPWP.Text) + ",FPCustKode = " + QuotedStr(tbFPCustKode.Text) + ", fgppn = " + QuotedStr(ddlPPN.SelectedValue) + ", " + _
                " FgLimit = " + QuotedStr(ddlFgLimit.SelectedValue) + ", " + _
                " contactName = " + QuotedStr(tbCName.Text) + ", contacttitle = " + QuotedStr(tbContactTitle.Text) + ", " + _
                " contacthp = " + QuotedStr(tbContactHp.Text) + ", ContactEmail = " + QuotedStr(tbCEmail.Text) + ", KodePPn = " + QuotedStr(ddlKodePPn.SelectedValue) + ", " + _
                " fgactive = " + QuotedStr(ddlAktive.SelectedValue) + ", " + _
                " Nik = " + QuotedStr(tbNik.Text) + ", " + _
                " Tempat = " + QuotedStr(tbTempat.Text) + ", " + _
                " WargaNegara = " + QuotedStr(ddlWargaNegara.SelectedValue) + ", " + _
                " TglLahir = '" + Format(tbDateLahir.SelectedValue, "yyyy-MM-dd") + "', " + _
                " CustName = '" + tbName.Text + "', UserDate = getDate()" + _
                " WHERE CustCode = '" + tbCode.Text + "'"
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlView.Visible = False
            PnlHd.Visible = True

            BindData(1)
            'BindData(2, Session("StrResult"))
        Catch ex As Exception
            Throw New Exception("Save Hd Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub DeleteHd(ByVal Nmbr As String)
        Dim SQLString As String
        Try
            SQLString = "UPDATE MsCustomer SET FgActive = 'N' where CustCode = '" & Nmbr & "'"
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
            ViewState("Dt") = SQLExecuteQuery("Select A.* from MsCustContact A WHERE CustCode = ''", ViewState("DBConnection").ToString).Tables(0)
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
            ViewState("DtAddr") = SQLExecuteQuery("Select A.* from MsCustAddress A WHERE A.CustCode = ''", ViewState("DBConnection").ToString).Tables(0)
            GridViewAddr.DataSource = ViewState("DtAddr")
            GridViewAddr.DataBind()
            ViewState("DtBillTo") = SQLExecuteQuery("Select A.* from MsCustBillTo A WHERE A.CustCode = ''", ViewState("DBConnection").ToString).Tables(0)
            GridViewBillTo.DataSource = ViewState("DtBillTo")
            GridViewBillTo.DataBind()
            ViewState("DtField") = SQLExecuteQuery("SELECT A.CustCode, A.CustField, A.UserId, A.UserDate FROM MsCustInfoField A WHERE A.CustCode = ''", ViewState("DBConnection").ToString).Tables(0)
            GridViewField.DataSource = ViewState("DtField")
            GridViewField.DataBind()
            ViewState("DtTax") = SQLExecuteQuery("Select A.* from MsCustAddress A WHERE A.CustCode = ''", ViewState("DBConnection").ToString).Tables(0)
            GridViewTax.DataSource = ViewState("DtTax")
            GridViewTax.DataBind()
        Catch ex As Exception
            Throw New Exception("new Record Error " + ex.ToString)
        End Try
    End Sub

    Private Sub ClearHd()
        Try
            tbCode.Text = ""
            tbName.Text = ""
            tbMember.Text = ""
            tbSister.Text = ""
            tbName.Text = ""

            ddlCustGroup.SelectedIndex = 0
            ddlCustType.SelectedIndex = 0
            tbAddress1.Text = ""
            tbAddress2.Text = ""
            tbPhone.Text = ""
            ddlCity.SelectedIndex = 0
            tbPostCode.Text = ""
            tbFax.Text = ""
            tbEmail.Text = ""
            ddlPPN.SelectedIndex = 1
            ddlKodePPn.SelectedIndex = 0
            tbNPWP.Text = ""
            tbFPCustKode.Text = ""
            ddlAktive.SelectedIndex = 0
            ddlPriceBySO.SelectedIndex = 0
            ddlPaymentTo.SelectedIndex = 0
            tbContactTitle.Text = ""
            tbContactHp.Text = ""
            tbContactName1.Text = ""
            tbContactEmail.Text = ""
            tbCEmail.Text = ""
            tbTerm.Text = ""
            tbCName.Text = ""
            ddlTerm.SelectedIndex = 0
            ddlFgLimit.SelectedValue = "Y"
            'tbUsedLimit.Text = 0
            'tbGracePeriod.Text = 0
            'tbMaxLImit.Text = 0
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
            tbContactName1.Text = ""
            tbTitle.Text = ""
            tbContactAddr1.Text = ""
            tbContactAddr2.Text = ""
            tbPostalCode.Text = ""
            tbTelephone.Text = ""
            tbContactFax.Text = ""
            tbContactEmail.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Error " + ex.ToString)
        End Try
    End Sub

    Private Sub cleardtField()
        Try
            ddlCustField.SelectedIndex = 0
        Catch ex As Exception
            Throw New Exception("Clear Dt Field Error " + ex.ToString)
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
                lbStatus.Text = "Customer Code must have value"
                tbCode.Focus()
                Return False
            End If
            If tbName.Text.Trim = "" Then
                lbStatus.Text = "Customer Name must have value"
                tbName.Focus()
                Return False
            End If
            If ddlAktive.SelectedValue.Trim = "" Then
                lbStatus.Text = "Customer Name must have value"
                tbName.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("Ceh Hd Error : " + ex.ToString)
        End Try
    End Function

    Function CekDt(ByVal Dr As DataRow) As Boolean
        Try
            If Dr.RowState = DataRowState.Deleted Then
                Return True
            End If
            If Dr("ContactName").ToString.Trim = "" Then
                lbStatus.Text = "Contact Name Must Have Value"
                tbContactName1.Focus()
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
                    PnlAddress.Visible = True
                    PnlBillTo.Visible = True
                    PnlField.Visible = True
                    pnlTax.Visible = True
                    btnAddDt.Visible = True
                    pnlEditDt.Visible = False
                    PnlEditAddress.Visible = False
                    PnlEditBillTo.Visible = False
                    PnlEditField.Visible = False
                    PnlEditTax.Visible = False
                    ViewState("CustCode") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    BindDataDt(ViewState("CustCode"))
                    GridViewAddr.PageIndex = 0
                    BindDataDtAddr(ViewState("CustCode"))
                    GridViewBillTo.PageIndex = 0
                    BindDataDtBillTo(ViewState("CustCode"))
                    GridViewField.PageIndex = 0
                    BindDataField(ViewState("CustCode"))
                    GridViewTax.PageIndex = 0
                    BindDataDtTax(ViewState("CustCode"))

                    FillTextBoxHd(ViewState("CustCode"))
                    
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
                    'If GVR.Cells(27).Text = "N" Then
                    '    lbStatus.Text = "Customer Is Not Active, Can't Update"
                    '    Exit Sub
                    'End If
                    If CheckMenuLevel("Edit") = False Then
                        Exit Sub
                    End If
                    PnlHd.Visible = False
                    pnlView.Visible = True
                    pnlDt.Visible = False
                    PnlAddress.Visible = False
                    PnlBillTo.Visible = False
                    PnlField.Visible = False
                    pnlTax.Visible = False
                    btnadddt.Visible = False
                    ViewState("CustCode") = GVR.Cells(2).Text
                    GridDt.PageIndex = 0
                    BindDataDt(ViewState("CustCode"))
                    FillTextBoxHd(ViewState("CustCode"))
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
                    ViewState("CustCode") = GVR.Cells(2).Text
                    DeleteHd(ViewState("CustCode"))
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
                PnlAddress.Visible = True
                PnlBillTo.Visible = True
                    PnlField.Visible = True
                    pnlTax.Visible = True
                btnadddt.Visible = True
                pnlEditDt.Visible = False
                PnlEditAddress.Visible = False
                PnlEditBillTo.Visible = False
                    PnlEditField.Visible = False
                    PnlEditTax.Visible = False
                ViewState("CustCode") = GVR.Cells(2).Text
                GridDt.PageIndex = 0
                BindDataDt(ViewState("CustCode"))
                GridViewAddr.PageIndex = 0
                BindDataDtAddr(ViewState("CustCode"))
                GridViewBillTo.PageIndex = 0
                BindDataDtBillTo(ViewState("CustCode"))
                GridViewField.PageIndex = 0
                BindDataField(ViewState("CustCode"))
                    GridViewTax.PageIndex = 0
                    BindDataDtTax(ViewState("CustCode"))
                    FillTextBoxHd(ViewState("CustCode"))
                ViewState("StateHd") = "Insert"
                ModifyInput(True)
                Menu1.Visible = True
                Menu1.Items.Item(0).Selected = True
                MultiView1.Visible = True
                MultiView1.ActiveViewIndex = 0
                btnSaveHd.Visible = True
                btnCancelHd.Visible = True
                btnReset.Visible = False
            End If
            End If

        Catch ex As Exception
            lbStatus.Text = "Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub FillTextBoxHd(ByVal Nmbr As String)
        Dim Dt As DataTable
        Try
            Dt = BindDataTransaction(GetStringHd, "Customer_Code = " + QuotedStr(Nmbr), ViewState("DBConnection").ToString)

            tbCode.Text = Nmbr
            BindToText(tbName, Dt.Rows(0)("Customer_Name").ToString)
            BindToText(tbMember, Dt.Rows(0)("MemberOfGroup").ToString)
            BindToText(tbSister, Dt.Rows(0)("SisterCompany").ToString)


            BindToDropList(ddlCustGroup, Dt.Rows(0)("Customer_Group").ToString)
            BindToDropList(ddlCustType, Dt.Rows(0)("Customer_Type").ToString)
            BindToDropList(ddlPPN, Dt.Rows(0)("PPN").ToString)
            BindToDropList(ddlKodePPn, Dt.Rows(0)("KodePPn").ToString)
            BindToText(tbAddress1, Dt.Rows(0)("Address1").ToString)
            BindToText(tbAddress2, Dt.Rows(0)("Address2").ToString)
            BindToDropList(ddlCity, Dt.Rows(0)("City").ToString)
            BindToText(tbPostCode, Dt.Rows(0)("Zip_Code").ToString)
            BindToText(tbPhone, Dt.Rows(0)("Phone").ToString)
            BindToText(tbFax, Dt.Rows(0)("Fax").ToString)
            BindToText(tbEmail, Dt.Rows(0)("Email").ToString)
            BindToDropList(ddlCurr, Dt.Rows(0)("Currency").ToString)
            BindToDropList(ddlPaymentTo, Dt.Rows(0)("PayCode").ToString)
            BindToText(tbTerm, Dt.Rows(0)("Term").ToString)
            BindToDropList(ddlTerm, Dt.Rows(0)("Term").ToString)
            BindToText(tbNPWP, Dt.Rows(0)("NPWP").ToString)
            BindToDropList(ddlAktive, Dt.Rows(0)("FgActive").ToString)
            BindToDropList(ddlPriceBySO, Dt.Rows(0)("PriceBySO").ToString)
            BindToText(tbCName, Dt.Rows(0)("Contact_Person").ToString)
            BindToText(tbContactTitle, Dt.Rows(0)("Contact_Title").ToString)
            BindToText(tbContactHp, Dt.Rows(0)("ContactHp").ToString)
            BindToText(tbCEmail, Dt.Rows(0)("Contact_Email").ToString)
            'BindToDropList(ddlFgLimit, Dt.Rows(0)("FgLimit").ToString)
            'BindToText(tbCustLimit, Dt.Rows(0)("CustLimit").ToString)
            'BindToText(tbMaxLImit, Dt.Rows(0)("UseLimit").ToString)
            'BindToText(tbUsedLimit, Dt.Rows(0)("UseLimit").ToString)
            'BindToText(tbBalanceLimit, Dt.Rows(0)("Balance").ToString)
            'BindToText(tbGracePeriod, Dt.Rows(0)("GracePeriod").ToString)
  

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
        Dim SQLString As String
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridDt.Rows(e.RowIndex)
        'dr = ViewState("Dt").Select("ItemNo = " + GVR.Cells(1).Text)
        'dr(0).Delete()

        SQLString = "Delete from MsCustContact where CustCode = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + GVR.Cells(1).Text
        SQLExecuteNonQuery(SQLString, ViewState("DBConnection"))
        'GridDt.DataSource = ViewState("Dt")
        BindDataDt(tbCode.Text)
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
            ddlContactType.SelectedValue = GVR.Cells(2).Text
            tbContactName1.Text = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbTitle.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbContactAddr1.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            tbContactAddr2.Text = GVR.Cells(6).Text.Replace("&nbsp;", "")
            tbPostalCode.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            tbTelephone.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            tbContactFax.Text = GVR.Cells(9).Text.Replace("&nbsp;", "")
            tbContactEmail.Text = GVR.Cells(10).Text.Replace("&nbsp;", "")
            pnlEditDt.Visible = True
            pnlDt.Visible = False
            ViewState("StateDt") = "Edit"
            ddlContactType.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid Dt Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbTerm_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbTerm.TextChanged
        Dim DsTerm As DataSet
        Dim DrTerm As DataRow
        Try
            DsTerm = SQLExecuteQuery("Select TermCode, TermName from MsTerm where TermCode = '" + tbTerm.Text + "'",ViewState("DBConnection").ToString)
            If DsTerm.Tables(0).Rows.Count = 1 Then
                DrTerm = DsTerm.Tables(0).Rows(0)
                tbTerm.Text = DrTerm("TermCode").ToString
                ddlTerm.SelectedValue = DrTerm("TermCode").ToString
            Else
                tbTerm.Text = ""
                ddlTerm.Text = ""
            End If
            tbNPWP.Focus()

        Catch ex As Exception
            lbStatus.Text = "ddl Term Error : " + ex.ToString
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

    Protected Sub ddlTerm_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTerm.SelectedIndexChanged
        Dim DsTerm As DataSet
        Dim DrTerm As DataRow
        Try
            DsTerm = SQLExecuteQuery("Select TermCode, TermName from MsTerm where TermCode= '" + ddlTerm.SelectedValue + "'",ViewState("DBConnection").ToString)
            If DsTerm.Tables(0).Rows.Count = 1 Then
                DrTerm = DsTerm.Tables(0).Rows(0)
                tbTerm.Text = DrTerm("TermCode").ToString
                ddlTerm.SelectedValue = DrTerm("TermCode").ToString
            Else
                tbTerm.Text = ""
                ddlTerm.Text = ""
            End If
            tbNPWP.Focus()

        Catch ex As Exception
            lbStatus.Text = "ddl Term Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnAddDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDt.Click
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
        tbContactName1.Focus()
        Exit Sub
    End Sub

    Private Sub SaveDt1()
        Dim SQLString As String
        Try
            If tbContactName1.Text.Trim = "" Then
                lbStatus.Text = "Contact Name Must Have Value"
                tbContactName1.Focus()
                Exit Sub
            End If
            If tbTitle.Text.Trim = "" Then
                lbStatus.Text = "Contact Type Must Have Value"
                tbTitle.Focus()
                Exit Sub
            End If

            If ViewState("StateDt") = "Insert" Then
                If SQLExecuteScalar("SELECT ItemNo FROM MsCustContact WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + lbItemNo.Text, ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "Detail Contact " + QuotedStr(lbItemNo.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsCustContact (CustCode, ItemNo, ContactType, ContactName, ContactTitle, Address1, Address2, ZipCode, Phone, Fax, Email, UserID, UserDate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + lbItemNo.Text + ", " + QuotedStr(ddlContactType.SelectedValue) + ", " + QuotedStr(tbContactName1.Text) + ", " + QuotedStr(tbTitle.Text) + ", " + QuotedStr(tbContactAddr1.Text) + ", " + QuotedStr(tbContactAddr2.Text) + ", " + _
                QuotedStr(tbPostalCode.Text) + ", " + QuotedStr(tbTelephone.Text) + ", " + QuotedStr(tbContactFax.Text) + ", " + QuotedStr(tbContactEmail.Text) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                SQLString = "UPDATE MsCustContact SET ContactName = " + QuotedStr(tbContactName1.Text) + ", ContactTitle = " + QuotedStr(tbTitle.Text) + ", " + _
                " Address1 = " + QuotedStr(tbContactAddr1.Text) + ", Address2= " + QuotedStr(tbContactAddr2.Text) + ", " + _
                " ZipCode = " + QuotedStr(tbPostalCode.Text) + ", ContactType = " + QuotedStr(ddlContactType.SelectedValue) + ", " + _
                " Phone = " + QuotedStr(tbTelephone.Text) + ", Fax = " + QuotedStr(tbContactFax.Text) + ", " + _
                " email = " + QuotedStr(tbContactEmail.Text) + ", UserDate = getDate()" + _
                " WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND ItemNo = " + QuotedStr(lbItemNo.Text) + " "
            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            pnlEditDt.Visible = False
            pnlDt.Visible = True
            EnableHd(GridDt.Rows.Count = 0)
            BindDataDt(ViewState("CustCode"))
            GridDt.DataSource = ViewState("Dt")
            GridDt.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Dt1 Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelAddr_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelAddr.Click
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
            lbCode.Text = GVR.Cells(1).Text
            tbDeliveryCode.Text = GVR.Cells(1).Text
            tbDeliveryName.Text = GVR.Cells(2).Text.Replace("&nbsp;", "")
            ddlDeliveryType.SelectedValue = GVR.Cells(3).Text.Replace("&nbsp;", "")
            tbAddress1Addr.Text = GVR.Cells(4).Text.Replace("&nbsp;", "")
            tbAddress2Addr.Text = GVR.Cells(5).Text.Replace("&nbsp;", "")
            'ddlCityAddr.SelectedValue = GVR.Cells(6).Text '.Replace("&nbsp;", "")
            tbZipCodeAddr.Text = GVR.Cells(7).Text.Replace("&nbsp;", "")
            TbPhoneAddr.Text = GVR.Cells(8).Text.Replace("&nbsp;", "")
            tbFaxAddr.Text = GVR.Cells(9).Text.Replace("&nbsp;", "")
            tbContactPersonAddr.Text = GVR.Cells(10).Text.Replace("&nbsp;", "")
            PnlEditAddress.Visible = True
            PnlAddress.Visible = False
            EnableHd(False)
            ViewState("StateDtAddr") = "Edit"
            tbAddress1Addr.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid View Addr Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewAddr_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewAddr.RowDeleting
        Dim SQLString As String
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridViewAddr.Rows(e.RowIndex)

        'dr = ViewState("DtAddr").Select("DeliveryCode = " + GVR.Cells(1).Text)
        'dr(0).Delete()
        'ViewState("DtAddr").AcceptChanges()
        SQLString = "Delete from MsCustAddress where CustCode = " + QuotedStr(tbCode.Text) + " AND DeliveryCode = " + QuotedStr(GVR.Cells(1).Text)
        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        'GridDt.DataSource = ViewState("Dt")
        BindDataDtAddr(tbCode.Text)
        'GridViewAddr.DataSource = ViewState("DtAddr")
        'GridViewAddr.DataBind()
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

    
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsearch.Click
        Try
            BindData(1)
            pnlNav.Visible = False
            pnlDt.Visible = False
            PnlAddress.Visible = False
            PnlBillTo.Visible = False
            PnlField.Visible = False
            pnlTax.Visible = False
        Catch ex As Exception
            lbStatus.Text = "Btn Search Error : " + ex.ToString
        End Try
    End Sub

    Private Sub cleardtAddr()
        Try
            tbDeliveryCode.Text = ""
            tbDeliveryName.Text = ""
            ddlDeliveryType.SelectedIndex = 0
            tbAddress1Addr.Text = ""
            tbAddress2Addr.Text = ""
            ddlCityAddr.SelectedIndex = 0
            tbZipCodeAddr.Text = ""
            TbPhoneAddr.Text = ""
            tbFaxAddr.Text = ""
            tbContactPersonAddr.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Addr Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddField_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddField.Click
        cleardtField()
        ' newTrans()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = "Customer Must Filled"
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
            Row = ViewState("DtField").select("")
            i = Row.Length
        End If

        ViewState("StateDtField") = "Insert"
        PnlEditField.Visible = True
        PnlField.Visible = False
        EnableHd(False)
        ddlCustField.Focus()
        Exit Sub
    End Sub

    Private Sub SaveDtField()
        Dim SQLString As String
        Try
            If ddlCustField.Text.Trim = "" Then
                lbStatus.Text = "Customer Field Name Must Have Value"
                ddlCustField.Focus()
                Exit Sub
            End If

            If ViewState("StateDtField") = "Insert" Then
                If SQLExecuteScalar("SELECT CustField FROM MsCustInfoField WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND CustField = " + QuotedStr(ddlCustField.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "Detail Field Info " + QuotedStr(ddlCustField.SelectedValue) + " has already been exist"
                    Exit Sub
                End If


                SQLString = "INSERT INTO MsCustinfoField (CustCode, CustField, UserID, UserDate) " + _
                "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(ddlCustField.SelectedValue) + ", " + _
                QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                If ddlCustField.SelectedValue <> lbCode.Text Then
                    If SQLExecuteScalar("SELECT CustField FROM MsCustInfoField WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND CustField = " + QuotedStr(ddlCustField.SelectedValue), ViewState("DBConnection").ToString).Length > 0 Then
                        lbStatus.Text = "Detail Field Info " + QuotedStr(ddlCustField.SelectedValue) + " has already been exist"
                        Exit Sub
                    End If
                End If

                SQLString = "UPDATE MsCustinfoField SET CustField = " + QuotedStr(ddlCustField.SelectedValue) + _
                ", UserDate = getDate()" + _
                " WHERE CustCode = '" + tbCode.Text + "' AND CustField = '" + lbCode.Text + "' "

            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditField.Visible = False
            PnlField.Visible = True
            EnableHd(GridViewField.Rows.Count = 0)
            BindDataField(ViewState("CustCode"))
            GridViewField.DataSource = ViewState("DtField")
            GridViewField.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Dt Field Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataField(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtField") = Nothing
            dt = SQLExecuteQuery(GetStringDtField(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtField") = dt
            GridViewField.DataSource = dt
            GridViewField.DataBind()
            BindGridDt(dt, GridViewField)
        Catch ex As Exception
            Throw New Exception("Bind Data Field Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveField_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveField.Click
        Try
            SaveDtField()
            PnlField.Visible = True
            PnlEditField.Visible = False
            GridViewField.PageIndex = 0
            BindDataField(ViewState("CustCode"))
        Catch ex As Exception
            lbStatus.Text = "Save Field Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelField_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelField.Click
        Try
            PnlField.Visible = True
            PnlEditField.Visible = False
            EnableHd(GridViewField.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Field Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewField_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewField.PageIndexChanging
        Try
            GridViewField.PageIndex = e.NewPageIndex
            GridViewField.DataSource = ViewState("DtField")
            GridViewField.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid View Filed Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridViewField_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewField.RowCommand
        Try
            If e.CommandName = "Insert" Then

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid View Field Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewField_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewField.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridViewField.Rows(e.NewEditIndex)
            'lbStatus.Text = ddlCustField.SelectedValue
            lbCode.Text = GVR.Cells(1).Text
            ddlCustField.SelectedValue = GVR.Cells(1).Text
            PnlEditField.Visible = True
            PnlField.Visible = False
            EnableHd(False)
            ViewState("StateDtField") = "Edit"
            ddlCustField.Focus()
        Catch ex As Exception
            lbStatus.Text = "Grid View Field Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Private Sub SaveDtAddr()
        Dim SQLString As String
        Try
            If tbDeliveryCode.Text.Trim = "" Then
                lbStatus.Text = "Delivery Code Must Have Value"
                tbDeliveryCode.Focus()
                Exit Sub
            End If
            If tbDeliveryName.Text.Trim = "" Then
                lbStatus.Text = "Delivery Name Must Have Value"
                tbDeliveryName.Focus()
                Exit Sub
            End If
            If tbAddress1Addr.Text.Trim = "" Then
                lbStatus.Text = "Delivery Address Must Have Value"
                tbAddress1Addr.Focus()
                Exit Sub
            End If
            If ddlCityAddr.Text.Trim = "" Then
                lbStatus.Text = "City Must Have Value"
                ddlCityAddr.Focus()
                Exit Sub
            End If

            If ViewState("StateDtAddr") = "Insert" Then
             
                If SQLExecuteScalar("SELECT DeliveryCode FROM MsCustAddress WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND DeliveryCode = " + QuotedStr(tbDeliveryCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "Detail Address " + QuotedStr(tbDeliveryCode.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsCustAddress (CustCode, DeliveryCode, DeliveryName, DeliveryType, DeliveryAddr1, DeliveryAddr2, City, ZipCode, PhoneNo, Fax, ContactPerson, UserId, UserDate) " + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbDeliveryCode.Text) + ", " + QuotedStr(tbDeliveryName.Text) + ", " + QuotedStr(ddlDeliveryType.SelectedValue) + ", " + QuotedStr(tbAddress1Addr.Text) + ", " + QuotedStr(tbAddress2Addr.Text) + ", " + _
               QuotedStr(ddlCityAddr.SelectedValue) + " , " + QuotedStr(tbZipCodeAddr.Text) + ", " + QuotedStr(TbPhoneAddr.Text) + ", " + QuotedStr(tbFaxAddr.Text) + ", " + QuotedStr(tbContactPersonAddr.Text) + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", getDate()"

            Else
                If lbCode.Text <> tbDeliveryCode.Text Then
                    If SQLExecuteScalar("SELECT DeliveryCode FROM MsCustAddress WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND DeliveryCode = " + QuotedStr(tbDeliveryCode.Text), ViewState("DBConnection").ToString).Length > 0 Then
                        lbStatus.Text = "Detail Address " + QuotedStr(tbDeliveryCode.Text) + " has already been exist"
                        Exit Sub
                    End If
                End If

                SQLString = "UPDATE MsCustAddress SET DeliveryCode = " + QuotedStr(tbDeliveryCode.Text) + ", DeliveryName = " + QuotedStr(tbDeliveryName.Text) + ", " + _
                " DeliveryType = " + QuotedStr(ddlDeliveryType.SelectedValue) + ", DeliveryAddr1= " + QuotedStr(tbAddress1Addr.Text) + ", " + _
                " DeliveryAddr2 = " + QuotedStr(tbAddress2Addr.Text) + ", City = " + QuotedStr(ddlCityAddr.SelectedValue) + ", " + _
                " ZipCode = " + QuotedStr(tbZipCodeAddr.Text) + ", PhoneNo = " + QuotedStr(TbPhoneAddr.Text) + ", Fax = " + QuotedStr(tbFaxAddr.Text) + ", " + _
                " ContactPerson = " + QuotedStr(tbContactPersonAddr.Text) + ", UserDate = getDate()" + _
                " WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND DeliveryCode = " + QuotedStr(lbCode.Text) + " "
                End If
            'lbStatus.Text = SQLString
            'Exit Sub

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditAddress.Visible = False
            PnlAddress.Visible = True
            GridViewAddr.PageIndex = 0
            BindDataDtAddr(ViewState("CustCode"))
        Catch ex As Exception
            Throw New Exception("Save Dt Addr Data Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDtAddr(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtAddr") = Nothing
            dt = SQLExecuteQuery(GetStringDtAddr(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtAddr") = dt
            GridViewAddr.DataSource = dt
            GridViewAddr.DataBind()
            BindGridDt(dt, GridViewAddr)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Addr Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveAddr_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveAddr.Click
        Try
            SaveDtAddr()
        Catch ex As Exception
            lbStatus.Text = "Save Addr Error : " + ex.ToString
        End Try
    End Sub

    Private Sub cleardtBillTo()
        Try
            tbBillTo.Text = ""
            tbBillToName.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Bill To Error " + ex.ToString)
        End Try
    End Sub

    Private Sub cleardtTax()
        Try
            tbTaxAddress.Text = ""
            tbTaxNpwp.Text = ""
        Catch ex As Exception
            Throw New Exception("Clear Dt Bill To Error " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddBillTo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddBillTo.Click
        cleardtBillTo()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = "Customer Must Filled"
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
            Row = ViewState("DtBillTo").select("")
            i = Row.Length
        End If

        ViewState("StateDtBillTo") = "Insert"
        PnlEditBillTo.Visible = True
        PnlBillTo.Visible = False
        EnableHd(False)
        'EnableAddress(False)
        tbBillTo.Focus()
        Exit Sub
    End Sub


    Protected Sub btnAddTax_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddTax.Click
        cleardtTax()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = "Customer Must Filled"
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
            Row = ViewState("DtTax").select("")
            i = Row.Length
        End If

        ViewState("StateDtTax") = "Insert"
        PnlEditTax.Visible = True
        pnlTax.Visible = False
        EnableHd(False)
        'EnableAddress(False)
        tbTaxAddress.Focus()
        Exit Sub
    End Sub

    Protected Sub btnSaveBillTo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveBillTo.Click
        Dim SQLString As String
        Try
            If tbBillTo.Text.Trim = "" Then
                lbStatus.Text = "Bill To Must Have Value"
                tbBillTo.Focus()
                Exit Sub
            End If
            If ViewState("StateDtBillTo") = "Insert" Then

                If SQLExecuteScalar("SELECT CustCollect FROM MsCustBillTo WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND CustCollect = " + QuotedStr(tbBillTo.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "Detail Address " + QuotedStr(tbBillTo.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsCustBillTo (CustCode, CustCollect, UserId, UserDate) " + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbBillTo.Text) + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                If lbCode.Text <> tbBillTo.Text Then
                    If SQLExecuteScalar("SELECT CustCollect FROM MsCustBillTo WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND CustCollect = " + QuotedStr(tbBillTo.Text), ViewState("DBConnection").ToString).Length > 0 Then
                        lbStatus.Text = "Detail Address " + QuotedStr(tbBillTo.Text) + " has already been exist"
                        Exit Sub
                    End If
                End If

                SQLString = "UPDATE MsCustBillTo SET CustCollect = " + QuotedStr(tbBillTo.Text) + ", UserDate = getDate()" + _
                " WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND CustCollect = " + QuotedStr(lbCode.Text) + " "

            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditBillTo.Visible = False
            PnlBillTo.Visible = True
            EnableHd(GridViewBillTo.Rows.Count = 0)
            BindDataDtBillTo(ViewState("CustCode"))
            GridViewBillTo.DataSource = ViewState("DtBillTo")
            GridViewBillTo.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Dt BillTo Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnSaveTax_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsaveTax.Click
        Dim SQLString As String
        Try
            If ViewState("StateDtTax") = "Insert" Then

                If SQLExecuteScalar("SELECT CustTaxAddress FROM MsCustTaxAddress WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND CustTaxAddress = " + QuotedStr(tbTaxAddress.Text), ViewState("DBConnection").ToString).Length > 0 Then
                    lbStatus.Text = "Detail Tax " + QuotedStr(tbTaxAddress.Text) + " has already been exist"
                    Exit Sub
                End If

                SQLString = "INSERT INTO MsCustTaxAddress (CustCode, CustTaxAddress, CustTaxNPWP, UserId, UserDate) " + _
               "SELECT " + QuotedStr(tbCode.Text) + ", " + QuotedStr(tbTaxAddress.Text) + ", " + QuotedStr(tbTaxNpwp.Text) + ", " + _
               QuotedStr(ViewState("UserId").ToString) + ", getDate()"
            Else
                If lbCode.Text <> tbTaxAddress.Text Then
                    If SQLExecuteScalar("SELECT CustTaxAddress FROM MsCustTaxAddress WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND CustTaxAddress = " + QuotedStr(tbTaxAddress.Text), ViewState("DBConnection").ToString).Length > 0 Then
                        lbStatus.Text = "Detail Tax " + QuotedStr(tbTaxAddress.Text) + " has already been exist"
                        Exit Sub
                    End If
                End If

                SQLString = "UPDATE MsCustTaxAddress SET CustTaxAddress = " + QuotedStr(tbTaxAddress.Text) + ", CustTaxNPWP = " + QuotedStr(tbTaxNpwp.Text) + ", UserDate = getDate()" + _
                " WHERE CustCode = " + QuotedStr(tbCode.Text) + " AND CustTaxAddress = " + QuotedStr(lbCode.Text) + " "

            End If
            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            PnlEditTax.Visible = False
            pnlTax.Visible = True
            EnableHd(GridViewTax.Rows.Count = 0)
            BindDataDtTax(ViewState("CustCode"))
            GridViewTax.DataSource = ViewState("DtTax")
            GridViewTax.DataBind()
        Catch ex As Exception
            Throw New Exception("Save Dt Tax Data Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnCancelBillTo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelBillTo.Click
        Try
            PnlBillTo.Visible = True
            PnlEditBillTo.Visible = False
            EnableHd(GridViewBillTo.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Bill To Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnCancelTax_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancelTax.Click
        Try
            pnlTax.Visible = True
            PnlEditTax.Visible = False
            EnableHd(GridViewTax.Rows.Count = 0)
        Catch ex As Exception
            lbStatus.Text = "btn Cancel Tax Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewBillTo_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewBillTo.RowEditing
        Dim GVR As GridViewRow
        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridViewBillTo.Rows(e.NewEditIndex)
            lbCode.Text = GVR.Cells(1).Text
            tbBillTo.Text = GVR.Cells(1).Text
            tbBillToName.Text = GVR.Cells(2).Text
            PnlEditBillTo.Visible = True
            PnlBillTo.Visible = False
            'EnableHd(False)
            ViewState("StateDtBillTo") = "Edit"
            tbBillTo.Focus()
            ' BtnGetDt.Enabled = False
        Catch ex As Exception
            lbStatus.Text = "Grid View Bill To Row Editing Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewTax_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewTax.RowEditing
        Dim GVR As GridViewRow
        Dim alamat As String

        Try
            If CheckMenuLevel("Edit") = False Then
                Exit Sub
            End If
            GVR = GridViewTax.Rows(e.NewEditIndex)

            alamat = GVR.Cells(1).Text
            If alamat = "&nbsp;" Then
                alamat = alamat.Replace("&nbsp;", "")
            End If

            lbCode.Text = GVR.Cells(1).Text
            tbTaxAddress.Text = alamat
            tbTaxNpwp.Text = GVR.Cells(2).Text

            PnlEditTax.Visible = True
            pnlTax.Visible = False

            'EnableHd(False)
            ViewState("StateDtTax") = "Edit"
            tbTaxAddress.Focus()
            ' BtnGetDt.Enabled = False
        Catch ex As Exception
            lbStatus.Text = "Grid View Bill To Row Editing Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridViewBillTo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewBillTo.RowCommand
        Try
            If e.CommandName = "Insert" Then

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid View Bill To Item Command Error : " + ex.ToString
        End Try
    End Sub
    Protected Sub GridViewTax_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewTax.RowCommand
        Try
            If e.CommandName = "Insert" Then

            End If
        Catch ex As Exception
            lbStatus.Text = "Grid View Bill To Item Command Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewBillTo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewBillTo.PageIndexChanging
        Try
            GridViewBillTo.PageIndex = e.NewPageIndex
            GridViewBillTo.DataSource = ViewState("DtBillTo")
            GridViewBillTo.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid View Bill To Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridViewTax_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewTax.PageIndexChanging
        Try
            GridViewTax.PageIndex = e.NewPageIndex
            GridViewTax.DataSource = ViewState("DtTax")
            GridViewTax.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid View Tax Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub
    Private Sub BindDataDtBillTo(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtBillTo") = Nothing
            dt = SQLExecuteQuery(GetStringDtBillTo(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtBillTo") = dt
            GridViewBillTo.DataSource = dt
            GridViewBillTo.DataBind()
            BindGridDt(dt, GridViewBillTo)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Addr Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub BindDataDtTax(ByVal Referens As String)
        Try
            Dim dt As New DataTable
            ViewState("DtTax") = Nothing
            dt = SQLExecuteQuery(GetStringDtTax(Referens), ViewState("DBConnection").ToString).Tables(0)
            ViewState("DtTax") = dt
            GridViewTax.DataSource = dt
            GridViewTax.DataBind()
            BindGridDt(dt, GridViewTax)
        Catch ex As Exception
            Throw New Exception("Bind Data Dt Addr Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnBillTo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBillTo.Click
        Dim FieldResult As String
        Try
            Session("filter") = "SELECT Customer_Code, Customer_Name FROM vmscustomer"
            Session("DBConnection") = ViewState("DBConnection")
            FieldResult = "Customer_Code, Customer_Name"
            Session("Column") = FieldResult.Split(",")
            ViewState("Sender") = "btnBillTo"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lbStatus.Text = "btn Bill To Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbBillTo_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbBillTo.TextChanged
        Dim DsBillTo As DataSet
        Dim DrBillTo As DataRow
        Try
            DsBillTo = SQLExecuteQuery("Select * from VMsCustomer WHERE Customer_Code = '" + tbBillTo.Text + "'", ViewState("DBConnection").ToString)
            If DsBillTo.Tables(0).Rows.Count = 1 Then
                DrBillTo = DsBillTo.Tables(0).Rows(0)
                tbBillTo.Text = DrBillTo("Customer_Code")
                tbBillToName.Text = DrBillTo("Customer_Name")
            Else
                tbBillTo.Text = ""
                tbBillToName.Text = ""
            End If
            tbBillTo.Focus()
        Catch ex As Exception
            Throw New Exception("tb Bill To Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub btnAddAddress_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAddress.Click
        cleardtAddr()
        If tbCode.Text.Trim = "" Then
            lbStatus.Text = "Customer Must Filled"
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
            Row = ViewState("DtAddr").select("")
            i = Row.Length
        End If

        ViewState("StateDtAddr") = "Insert"
        PnlEditAddress.Visible = True
        PnlAddress.Visible = False
        EnableHd(False)
        tbDeliveryCode.Focus()
        Exit Sub
    End Sub

    Protected Sub btnSaveDt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveDt.Click
        Try
            SaveDt1()
        Catch ex As Exception
            lbStatus.Text = "Save Contact Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub GridViewBillTo_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewBillTo.RowDeleting
        Dim SQLString As String
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridViewBillTo.Rows(e.RowIndex)
        'dr = ViewState("DtBillTo").Select("CustCollect = " + GVR.Cells(1).Text)
        'dr(0).Delete()

        SQLString = "Delete from MsCustBillTo where CustCode = " + QuotedStr(tbCode.Text) + " AND CustCollect = " + QuotedStr(GVR.Cells(1).Text)
        SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
        BindDataDtBillTo(tbCode.Text)
        'GridViewBillTo.DataSource = ViewState("DtBillTo")
        'GridViewBillTo.DataBind()
        lbStatus.Text = "Data Deleted"
        EnableHd(GridViewBillTo.Rows.Count = 0)
    End Sub

    Protected Sub GridViewField_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewField.RowDeleting
        Dim SQlString As String
        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridViewField.Rows(e.RowIndex)
        'dr = ViewState("DtField").Select("CustField = " + GVR.Cells(1).Text)
        'dr(0).Delete()

        SQlString = "Delete from mscustinfofield where CustCode = " + QuotedStr(tbCode.Text) + " AND CustField = " + QuotedStr(GVR.Cells(1).Text)
        SQLExecuteNonQuery(SQlString, ViewState("DBConnection").ToString)
        BindDataField(tbCode.Text)

        'ViewState("DtField").AcceptChanges()
        'GridViewField.DataSource = ViewState("DtField")
        'GridViewField.DataBind()
        lbStatus.Text = "Data Deleted"
        EnableHd(GridViewField.Rows.Count = 0)
    End Sub

    Protected Sub GridViewAddr_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewAddr.PageIndexChanging
        Try
            GridViewAddr.PageIndex = e.NewPageIndex
            GridViewAddr.DataSource = ViewState("DtAddr")
            GridViewAddr.DataBind()
        Catch ex As Exception
            lbStatus.Text = "Grid View Addr Page Index Change Error : " + ex.ToString + " page index : " + e.NewPageIndex.ToString
        End Try
    End Sub

    Protected Sub GridViewTax_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewTax.RowDeleting
        Dim SQlString As String
        Dim alamat As String

        If CheckMenuLevel("Delete") = False Then
            Exit Sub
        End If
        Dim GVR As GridViewRow = GridViewTax.Rows(e.RowIndex)

        alamat = GVR.Cells(1).Text
        If alamat = "&nbsp;" Then
            alamat = alamat.Replace("&nbsp;", "")
        End If

        SQlString = "Delete from mscustTaxAddress where CustCode = " + QuotedStr(tbCode.Text) + " AND CustTaxAddress = " + QuotedStr(alamat)
        SQLExecuteNonQuery(SQlString, ViewState("DBConnection").ToString)
        BindDataDtTax(tbCode.Text)

        EnableHd(GridViewTax.Rows.Count = 0)

        lbStatus.Text = MessageDlg("Data Deleted")
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
        Dim i As Integer
        Dim paramgo As String
        'Make the selected menu item reflect the correct imageurl
        
        For i = 0 To Menu1.Items.Count - 1
            If i = e.Item.Value Then
                'Menu1.Items(i).ImageUrl = "selectedtab.gif"
            ElseIf Menu1.Items(5).Selected = True Then
                Dim GVR As GridViewRow = Nothing
                Try
                    If CheckMenuLevel("Insert") = False Then
                        Exit Sub
                    End If
                    paramgo = tbCode.Text + "|" + tbName.Text
                    If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                        Session("DBConnection") = ViewState("DBConnection")
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenAssign('Customer', '" + Request.QueryString("KeyId") + "','" + tbCode.Text + "|" + tbName.Text + "','AssCustProduct');", True)
                    End If
                Catch ex As Exception
                    lbStatus.Text = "DDL.SelectedValue = 4 Error : " + ex.ToString
                End Try
            End If
        Next
    End Sub

    Protected Sub btnprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnprint.Click
        Try
            Session("DBConnection") = ViewState("DBConnection")
            Session("SelectCommand") = "EXEC S_MKRptCustomerList '','','',0,'','','',''," + QuotedStr(ViewState("UserId").ToString)
            Session("ReportFile") = Server.MapPath("~\Rpt\RptCustList.frx")
            'lbStatus.Text = Session("SelectCommand")
            AttachScript("openprintdlg();", Page, Me.GetType)
        Catch ex As Exception
            lbStatus.Text = "btn Print Error : " + ex.ToString
        End Try
    End Sub
End Class