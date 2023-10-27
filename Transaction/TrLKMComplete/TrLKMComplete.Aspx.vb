Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Partial Class Transaction_TrLKMComplete_TrLKMComplete
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitProperty()
                ' FillCombo(ddlYear, "S_PLPanenPremiHadirGetYear", False, "Year", "Year", ViewState("DBConnection"))

                DataGrid.PageSize = CInt(ViewState("PageSizeGrid"))

                If Request.QueryString("ContainerId").ToString = "TrLKMCompleteID" Then
                    ddlComp.SelectedValue = "Asisten"
                    BindData()
                ElseIf Request.QueryString("ContainerId").ToString = "TrLKMCompleteAuditID" Then
                    ddlComp.SelectedValue = "Audit"
                    BindDataAudit()
                Else
                    ddlComp.SelectedValue = "Denda"
                    BindDataDenda()
                End If
                lbltitle.Text = "L K M Complete " + ddlComp.SelectedValue


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

        Dim GVR As GridViewRow
        Dim txtQty, txtDisc, txtAsistenDisc As TextBox
        Dim tempDS As New DataSet()
        Dim StrFilter As String

        Try



            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If
            tempDS = SQLExecuteQuery("EXEC S_PLWOResultSearchMulti " + QuotedStr(ddlComp.SelectedValue.ToString) + ", " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)

            DataGrid.DataSource = tempDS.Tables(0)
            DataGrid.DataBind()

            For Each GVR In DataGrid.Rows
                txtQty = GVR.FindControl("AsistenQty")
                txtDisc = GVR.FindControl("AsistenDiscPercent")
                txtAsistenDisc = GVR.FindControl("AsistenDisc")
                txtQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
                txtDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
                txtAsistenDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Next


            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransNmbr DESC"
            End If
            pnlCompleteAsisten.Visible = True
            PnlCompleteDenda.Visible = False
            PnlCompleteAudit.Visible = False





        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindDataAudit()
        Dim GVR As GridViewRow
        Dim txtAuditQty, txtAuditDisc, txtAuditDiscPercent As TextBox
        Dim tempDS As New DataSet()
        Dim StrFilter As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If
            tempDS = SQLExecuteQuery("EXEC S_PLWOResultSearchMulti " + QuotedStr(ddlComp.SelectedValue.ToString) + ", " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)

            DataGridAudit.DataSource = tempDS.Tables(0)
            DataGridAudit.DataBind()

            For Each GVR In DataGridAudit.Rows
                txtAuditQty = GVR.FindControl("AuditQty")
                txtAuditDisc = GVR.FindControl("AuditDisc")
                txtAuditDiscPercent = GVR.FindControl("AuditDiscPercent")
                txtAuditQty.Attributes.Add("OnKeyDown", "return PressNumeric();")
                txtAuditDisc.Attributes.Add("OnKeyDown", "return PressNumeric();")
                txtAuditDiscPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Next

            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransNmbr DESC"
            End If
            pnlCompleteAsisten.Visible = False
            PnlCompleteDenda.Visible = False
            PnlCompleteAudit.Visible = True

        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub

    Sub BindDataDenda()
        Dim GVR As GridViewRow
        Dim txtDendaAmount, txtDendaManagerPercent, txtDendaAsistenPercent, txtDendaAskepPercent, txtDendaMandorPercent As TextBox
        Dim tempDS As New DataSet()
        Dim StrFilter As String

        Try
            StrFilter = GenerateFilterMs(ddlField.SelectedValue, ddlField2.SelectedValue, tbFilter.Text, tbfilter2.Text, ddlNotasi.SelectedValue)
            If StrFilter.Length > 5 Then
                StrFilter = StrFilter.Remove(1, 5)
                StrFilter = " And " + StrFilter
            End If
            tempDS = SQLExecuteQuery("EXEC S_PLWOResultSearchMulti " + QuotedStr(ddlComp.SelectedValue.ToString) + ", " + QuotedStr(StrFilter) + ", " + QuotedStr(ViewState("UserId")), ViewState("DBConnection").ToString)

            DataGridDenda.DataSource = tempDS.Tables(0)
            DataGridDenda.DataBind()

            For Each GVR In DataGridDenda.Rows
                txtDendaAmount = GVR.FindControl("DendaAmount")
                txtDendaManagerPercent = GVR.FindControl("DendaManagerPercent")
                txtDendaAsistenPercent = GVR.FindControl("DendaAsistenPercent")
                txtDendaAskepPercent = GVR.FindControl("DendaAskepPercent")
                txtDendaMandorPercent = GVR.FindControl("DendaMandorPercent")

                txtDendaAmount.Attributes.Add("OnKeyDown", "return PressNumeric();")
                txtDendaManagerPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
                txtDendaAsistenPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
                txtDendaAskepPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")
                txtDendaMandorPercent.Attributes.Add("OnKeyDown", "return PressNumeric();")

            Next


            If ViewState("SortExpression") = Nothing Then
                ViewState("SortExpression") = "TransNmbr DESC"
            End If
            pnlCompleteAsisten.Visible = False
            PnlCompleteAudit.Visible = False
            PnlCompleteDenda.Visible = True



            '
        Catch ex As Exception
            Throw New Exception("Bind Data Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Try
            If ddlComp.SelectedValue = "Asisten" Then
                bindDataProcess()
            ElseIf ddlComp.SelectedValue = "Audit" Then
                bindDataProcessAudit()
            Else
                bindDataProcessDenda()
            End If

            ' BindData(ddlComp.SelectedValue )
        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub
    Protected Sub BtnUnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnApply.Click
        Try
            If ddlComp.SelectedValue = "Asisten" Then
                bindDataUnProcess()
            ElseIf ddlComp.SelectedValue = "Audit" Then
                bindDataUnProcessAudit()
            Else
                bindDataUnProcessDenda()
            End If

            ' BindData(ddlComp.SelectedValue )
        Catch ex As Exception
            lbstatus.Text = "btn apply error : " + ex.ToString
        End Try
    End Sub
    Private Sub bindDataProcess()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbTrans, lbWONO, lbtype, lbBlock, lbQtyLKM, lbprice, lbbruto, lbnetto As Label
            Dim tbQty, tbdisc, tbdiscforex As TextBox
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbTrans = GVR.FindControl("TransNmbr")
                lbWONO = GVR.FindControl("WONo")
                lbtype = GVR.FindControl("Type")
                lbBlock = GVR.FindControl("DivisiBlock")
                lbQtyLKM = GVR.FindControl("QtyLKM")
                lbprice = GVR.FindControl("AsistenPrice")
                lbbruto = GVR.FindControl("AsistenBruto")
                lbnetto = GVR.FindControl("AsistenNetto")
                tbQty = GVR.FindControl("AsistenQty")
                tbdisc = GVR.FindControl("AsistenDiscPercent")
                tbdiscforex = GVR.FindControl("AsistenDisc")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOResultCompleteAsisten " + QuotedStr(lbTrans.Text) + _
                    ", " + QuotedStr(lbWONO.Text) + ", " + QuotedStr(lbtype.Text) + ", " + QuotedStr(lbBlock.Text) + ", " + QuotedStr(ViewState("ServerDate")) + ", " + QuotedStr(lbQtyLKM.Text.Replace(",", "")) + ", " + QuotedStr(tbQty.Text.Replace(",", "")) + _
                    ", " + QuotedStr(lbprice.Text.Replace(",", "")) + ", " + QuotedStr(lbbruto.Text.Replace(",", "")) + ", " + QuotedStr(tbdisc.Text.Replace(",", "")) + ", " + QuotedStr(tbdiscforex.Text.Replace(",", "")) + ", " + QuotedStr(lbnetto.Text.Replace(",", "")) + _
                    ", " + QuotedStr(tbPIC.Text) + ", " + QuotedStr(tbRemark1.Text) + ", " + QuotedStr(tbRemark2.Text) + ", " + QuotedStr(tbRemark3.Text) + _
                    ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If
                End If
            Next
            BindData()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check Week for Process Premi"
                Exit Sub
            Else
                lbstatus.Text = "Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataUnProcess()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbTrans, lbWONO, lbtype, lbBlock, lbQtyLKM As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGrid.Rows
                CB = GVR.FindControl("cbSelect")
                lbTrans = GVR.FindControl("TransNmbr")
                lbWONO = GVR.FindControl("WONo")
                lbtype = GVR.FindControl("Type")
                lbBlock = GVR.FindControl("DivisiBlock")
                lbQtyLKM = GVR.FindControl("QtyLKM")


                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOResultUnCompleteAsisten " + QuotedStr(lbTrans.Text) + _
                    ", " + QuotedStr(lbWONO.Text) + ", " + QuotedStr(lbtype.Text) + ", " + QuotedStr(lbBlock.Text) + _
                    ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "

                    ' SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        Exit For
                    End If
                End If
            Next
            BindData()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check Un Complete"
                Exit Sub
            Else
                lbstatus.Text = "Un Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub

    Private Sub bindDataUnProcessAudit()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbTrans, lbWONO, lbtype, lbBlock, lbQtyLKM As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGridAudit.Rows
                CB = GVR.FindControl("cbSelect")
                lbTrans = GVR.FindControl("TransNmbr")
                lbWONO = GVR.FindControl("WONo")
                lbtype = GVR.FindControl("Type")
                lbBlock = GVR.FindControl("DivisiBlock")
                lbQtyLKM = GVR.FindControl("QtyLKM")


                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOResultUnCompleteAudit " + QuotedStr(lbTrans.Text) + _
                    ", " + QuotedStr(lbWONO.Text) + ", " + QuotedStr(lbtype.Text) + ", " + QuotedStr(lbBlock.Text) + _
                    ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "

                    'SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If
                End If
            Next
            BindDataAudit()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check Un Complete"
                Exit Sub
            Else
                lbstatus.Text = "Un Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub
    Private Sub bindDataUnProcessDenda()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbTrans, lbWONO, lbtype, lbBlock, lbQtyLKM As Label
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGridDenda.Rows
                CB = GVR.FindControl("cbSelect")
                lbTrans = GVR.FindControl("TransNmbr")
                lbWONO = GVR.FindControl("WONo")
                lbtype = GVR.FindControl("Type")
                lbBlock = GVR.FindControl("DivisiBlock")
                lbQtyLKM = GVR.FindControl("QtyLKM")


                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOResultUnCompleteDenda " + QuotedStr(lbTrans.Text) + _
                    ", " + QuotedStr(lbWONO.Text) + ", " + QuotedStr(lbtype.Text) + ", " + QuotedStr(lbBlock.Text) + _
                    ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "

                    'SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If
                End If
            Next
            BindDataDenda()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check Un Complete"
                Exit Sub
            Else
                lbstatus.Text = "Un Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If ddlComp.SelectedValue = "Asisten" Then
                DataGrid.PageIndex = 0
                DataGrid.EditIndex = -1
                BindData()
            ElseIf ddlComp.SelectedValue = "Audit" Then
                DataGridAudit.PageIndex = 0
                DataGridAudit.EditIndex = -1
                BindDataAudit()
            Else
                DataGridDenda.PageIndex = 0
                DataGridDenda.EditIndex = -1
                BindDataDenda()
            End If
        Catch ex As Exception
            lbstatus.Text = "Btn Search Error : " + vbCrLf + ex.ToString
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

    Public Sub ExportGridToExcel(ByVal filenamevalue As String)
        Dim form As HtmlForm = New HtmlForm()
        Dim worksheetname As String
        worksheetname = Left(filenamevalue, 31)
        Dim attachment As String '= "attachment; filename=PrintDetails.xls"
        attachment = "attachment; filename=" + filenamevalue + ".xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/ms-excel"
        'namespace (using system.IO)      
        Dim stw As StringWriter = New StringWriter()
        Dim htextw As HtmlTextWriter = New HtmlTextWriter(stw)
        GridExport.Parent.Controls.Add(form)
        form.Attributes("runat") = "server"
        form.Controls.Add(GridExport)
        Me.Controls.Add(form)
        form.RenderControl(htextw)
        Response.Write(stw.ToString())
        Response.End()
    End Sub

    Protected Sub cbSelectHd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlComp.SelectedValue = "Asisten" Then
            CheckAllDt(DataGrid, sender)
        ElseIf ddlComp.SelectedValue = "Audit" Then
            CheckAllDt(DataGridAudit, sender)
        Else
            CheckAllDt(DataGridDenda, sender)
        End If
    End Sub

    Protected Sub cbSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox
        Dim GRW As GridViewRow
        Dim Asisten, transnmbr As Label
        Try
            cb = sender
            For Each GRW In DataGrid.Rows
                Asisten = GRW.FindControl("CompleteAsisten")
                transnmbr = GRW.FindControl("TransNmbr")
                'lbstatus.Text = MessageDlg(Asisten.Text)
                'Exit Sub

                If cb.Checked = True And Asisten.Text = "Y" Then
                    BtnUnApply.Enabled = True
                    BtnApply.Enabled = False

                ElseIf cb.Checked = True And Asisten.Text = "N" Then
                    BtnUnApply.Enabled = False
                    BtnApply.Enabled = True

                Else
                    BtnUnApply.Enabled = True
                    BtnApply.Enabled = True

                End If

            Next

        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub CheckAllDt(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
                If cb.Checked = False Then
                    'btnGetSetZero.Visible = True
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub DataGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGrid.PageIndexChanging
        DataGrid.PageIndex = e.NewPageIndex
        BindData()
    End Sub

    Protected Sub ddlComp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlComp.SelectedIndexChanged
        Try
            If ddlComp.SelectedValue = "Asisten" Then
                DataGrid.PageIndex = 0
                DataGrid.EditIndex = -1
                BindData()
                'ElseIf ddlComp.SelectedValue = "Audit" Then
                '    DataGridAudit.PageIndex = 0
                '    DataGridAudit.EditIndex = -1
                '    BindDataAudit()
                'Else
                '    DataGridDenda.PageIndex = 0
                '    DataGridDenda.EditIndex = -1
                '    BindDataDenda()
            End If

        Catch ex As Exception
            lbstatus.Text = "ddlComp_SelectedIndexChanged = " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub AsistenQty_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim GVR As GridViewRow
            Dim lbQtyLKM, lbAsistenBruto, lbAsistenPrice As Label
            Dim lbAsistenNetto As Label
            Dim tbQty, tbAsistenDiscPercent, tbAsistenDisc As TextBox

            For Each GVR In DataGrid.Rows
                lbQtyLKM = GVR.FindControl("QtyLKM")
                tbQty = GVR.FindControl("AsistenQty")
                lbAsistenBruto = GVR.FindControl("AsistenBruto")
                lbAsistenPrice = GVR.FindControl("AsistenPrice")
                lbAsistenNetto = GVR.FindControl("AsistenNetto")
                tbAsistenDisc = GVR.FindControl("AsistenDisc")
                tbAsistenDiscPercent = GVR.FindControl("AsistenDiscPercent")

                If tbQty.Text > CFloat(lbQtyLKM.Text) Then
                    lbstatus.Text = "Qty Asisten must be lower than Qty LKM"
                    tbQty.Text = lbQtyLKM.Text
                    Exit For
                End If

                If tbAsistenDiscPercent.Text <> "0" Then
                    tbAsistenDisc.Text = FormatNumber((lbAsistenBruto.Text * (tbAsistenDiscPercent.Text / 100)), 2)
                End If
                lbAsistenBruto.Text = tbQty.Text * lbAsistenPrice.Text
                lbAsistenNetto.Text = lbAsistenBruto.Text - tbAsistenDisc.Text
            Next

        Catch ex As Exception
            lbstatus.Text = "AsistenQty_OnTextChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub AsistenDiscPercent_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim GVR As GridViewRow
            Dim lbQtyLKM, lbAsistenBruto, lbAsistenPrice As Label
            Dim lbAsistenNetto As Label
            Dim tbQty, tbAsistenDiscPercent, tbAsistenDisc As TextBox

            For Each GVR In DataGrid.Rows
                lbQtyLKM = GVR.FindControl("QtyLKM")
                tbQty = GVR.FindControl("AsistenQty")
                lbAsistenBruto = GVR.FindControl("AsistenBruto")
                lbAsistenPrice = GVR.FindControl("AsistenPrice")
                lbAsistenNetto = GVR.FindControl("AsistenNetto")
                tbAsistenDisc = GVR.FindControl("AsistenDisc")
                tbAsistenDiscPercent = GVR.FindControl("AsistenDiscPercent")

                'If tbAsistenDiscPercent.Text <> 0 Then
                '    tbAsistenDisc.Text = lbAsistenPrice.Text * (tbAsistenDiscPercent.Text / 100)
                'End If
                'lbAsistenNetto.Text = lbAsistenPrice.Text - tbAsistenDisc.Text

                If tbAsistenDiscPercent.Text <> 0 Then
                    tbAsistenDisc.Text = FormatNumber(lbAsistenBruto.Text * (tbAsistenDiscPercent.Text / 100), 2)
                End If
                lbAsistenNetto.Text = FormatNumber((lbAsistenBruto.Text - tbAsistenDisc.Text), 2)
            Next
        Catch ex As Exception
            lbstatus.Text = "AsistenDiscPercent_OnTextChanged " + vbCrLf + ex.ToString
        End Try
    End Sub


    Protected Sub AsistenDisc_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim GVR As GridViewRow
            Dim lbQtyLKM, lbAsistenBruto, lbAsistenPrice As Label
            Dim lbAsistenNetto As Label
            Dim tbQty, tbAsistenDiscPercent, tbAsistenDisc As TextBox

            For Each GVR In DataGrid.Rows
                lbQtyLKM = GVR.FindControl("QtyLKM")
                tbQty = GVR.FindControl("AsistenQty")
                lbAsistenBruto = GVR.FindControl("AsistenBruto")
                lbAsistenPrice = GVR.FindControl("AsistenPrice")
                lbAsistenNetto = GVR.FindControl("AsistenNetto")
                tbAsistenDisc = GVR.FindControl("AsistenDisc")
                tbAsistenDiscPercent = GVR.FindControl("AsistenDiscPercent")
                If tbAsistenDisc.Text <> "0" Then
                    tbAsistenDiscPercent.Text = FormatNumber((tbAsistenDisc.Text / lbAsistenBruto.Text) * 100)
                End If
                lbAsistenNetto.Text = lbAsistenBruto.Text - tbAsistenDisc.Text
            Next
        Catch ex As Exception
            lbstatus.Text = "AsistenDiscPercent_OnTextChanged " + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub AuditQty_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim GVR As GridViewRow
            Dim lbQtyLKM, lbAuditBruto, lbAuditPrice As Label
            Dim lbAuditNetto As Label
            Dim tbQty, tbAuditDiscPercent, tbAuditDisc As TextBox

            For Each GVR In DataGridAudit.Rows
                lbQtyLKM = GVR.FindControl("QtyLKM")
                tbQty = GVR.FindControl("AuditQty")
                lbAuditBruto = GVR.FindControl("AuditBruto")
                lbAuditPrice = GVR.FindControl("AuditPrice")
                lbAuditNetto = GVR.FindControl("AuditNetto")
                tbAuditDisc = GVR.FindControl("AuditDisc")
                tbAuditDiscPercent = GVR.FindControl("AuditDiscPercent")

                If tbQty.Text > CFloat(lbQtyLKM.Text) Then
                    lbstatus.Text = "Qty Audit must be lower than Qty LKM"
                    tbQty.Text = lbQtyLKM.Text
                    Exit For
                End If
                If tbAuditDiscPercent.Text <> "0" Then
                    tbAuditDisc.Text = lbAuditBruto.Text * (tbAuditDiscPercent.Text / 100)
                End If
                lbAuditBruto.Text = tbQty.Text * lbAuditPrice.Text
                lbAuditNetto.Text = lbAuditBruto.Text - tbAuditDisc.Text
            Next

        Catch ex As Exception
            lbstatus.Text = "AuditQty_OnTextChanged =" + vbCrLf + ex.ToString
        End Try
    End Sub
    Protected Sub AuditDiscPercent_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim GVR As GridViewRow
            Dim lbQtyLKM, lbAuditBruto, lbAuditPrice As Label
            Dim lbAuditNetto As Label
            Dim tbQty, tbAuditDiscPercent, tbAuditDisc As TextBox

            For Each GVR In DataGridAudit.Rows
                lbQtyLKM = GVR.FindControl("QtyLKM")
                tbQty = GVR.FindControl("AuditQty")
                lbAuditBruto = GVR.FindControl("AuditBruto")
                lbAuditPrice = GVR.FindControl("AuditPrice")
                lbAuditNetto = GVR.FindControl("AuditNetto")
                tbAuditDisc = GVR.FindControl("AuditDisc")
                tbAuditDiscPercent = GVR.FindControl("AuditDiscPercent")
                If tbAuditDiscPercent.Text <> 0 Then
                    tbAuditDisc.Text = lbAuditBruto.Text * (tbAuditDiscPercent.Text / 100)
                End If
                lbAuditNetto.Text = lbAuditBruto.Text - tbAuditDisc.Text
            Next
        Catch ex As Exception
            lbstatus.Text = "AuditDiscPercent_OnTextChanged " + vbCrLf + ex.ToString
        End Try
    End Sub


    Protected Sub AuditDisc_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim GVR As GridViewRow
            Dim lbQtyLKM, lbAuditBruto, lbAuditPrice As Label
            Dim lbAuditNetto As Label
            Dim tbQty, tbAuditDiscPercent, tbAuditDisc As TextBox

            For Each GVR In DataGridAudit.Rows
                lbQtyLKM = GVR.FindControl("QtyLKM")
                tbQty = GVR.FindControl("AuditQty")
                lbAuditBruto = GVR.FindControl("AuditBruto")
                lbAuditPrice = GVR.FindControl("AuditPrice")
                lbAuditNetto = GVR.FindControl("AuditNetto")
                tbAuditDisc = GVR.FindControl("AuditDisc")
                tbAuditDiscPercent = GVR.FindControl("AuditDiscPercent")
                If tbAuditDisc.Text <> "0" Then
                    tbAuditDiscPercent.Text = (tbAuditDisc.Text / lbAuditBruto.Text) * 100
                End If
                lbAuditNetto.Text = lbAuditBruto.Text - tbAuditDisc.Text
            Next
        Catch ex As Exception
            lbstatus.Text = "AuditDiscPercent_OnTextChanged " + vbCrLf + ex.ToString
        End Try
    End Sub

    Protected Sub DendaManager_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim dr As DataRow
        ''Dim ds As DataSet
        'Dim ds As New DataSet()
        'Dim Acc, tb, AccName As TextBox

        'Dim Count As Integer
        'Dim dgi As GridViewRow
        'Try
        'tb = sender
        'If tb.ID = "DendaManager" Then
        '    Count = DataGridDenda.Controls(0).Controls.Count
        '    dgi = DataGridDenda.Controls(0).Controls(Count - 2) '-1 for allowpaging = False
        '    Acc = dgi.FindControl("DendaManager")
        '    AccName = dgi.FindControl("DendaManagerName")
        '    ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Emp_No = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)
        'End If

        'If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
        '    Acc.Text = ""
        '    AccName.Text = ""
        'Else
        '    dr = ds.Tables(0).Rows(0)
        '    Acc.Text = dr("Emp_No").To\String
        '    AccName.Text = dr("Emp_Name").ToString
        'End If

        Dim GVR As GridViewRow
        Dim ds As New DataSet()
        Dim Acc, AccName As TextBox
        Dim dr As DataRow

        Try

            For Each GVR In DataGridDenda.Rows
                Acc = GVR.FindControl("DendaManager")
                AccName = GVR.FindControl("DendaManagerName")
                ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Emp_No = " + QuotedStr(Acc.Text), ViewState("DBConnection").ToString)

                If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                    Acc.Text = ""
                    AccName.Text = ""
                Else
                    dr = ds.Tables(0).Rows(0)
                    Acc.Text = dr("Emp_No").ToString
                    AccName.Text = dr("Emp_Name").ToString
                End If
            Next

        Catch ex As Exception
            lbstatus.Text = "DendaManager_OnTextChanged Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DendaAskep_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim GVR As GridViewRow
        Dim ds As New DataSet()
        Dim DendaAskep, DendaAskepName As TextBox
        Dim Dr As DataRow

        Try
            For Each GVR In DataGridDenda.Rows
                DendaAskep = GVR.FindControl("DendaAskep")
                DendaAskepName = GVR.FindControl("DendaAskepName")
                ds = SQLExecuteQuery("Select Emp_No, Emp_Name FROM V_MsEmployee WHERE Emp_No = " + QuotedStr(DendaAskep.Text), ViewState("DBConnection").ToString)

                If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                    DendaAskep.Text = ""
                    DendaAskepName.Text = ""
                    lbstatus.Text = "Account Denda Askep not Define"
                Else
                    Dr = ds.Tables(0).Rows(0)
                    DendaAskep.Text = Dr("Emp_No").ToString
                    DendaAskepName.Text = Dr("Emp_Name").ToString
                End If
            Next

        Catch ex As Exception
            lbstatus.Text = "DendaAskep_OnTextChanged Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DendaAsisten_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVR As GridViewRow
        Dim Ds As New DataSet()
        Dim DendaAsisten, DendaAsistenName As TextBox
        Dim Dr As DataRow
        Try
            For Each GVR In DataGridDenda.Rows
                DendaAsisten = GVR.FindControl("DendaAsisten")
                DendaAsistenName = GVR.FindControl("DendaAsistenName")
                Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name FROM V_MsEmployee WHERE Emp_no = " + QuotedStr(DendaAsisten.Text), ViewState("DBConnection").ToString)

                If IsNothing(Ds) Or Ds.Tables(0).Rows.Count <= 0 Or IsNothing(Ds.Tables(0)) Then
                    DendaAsisten.Text = ""
                    DendaAsistenName.Text = ""
                Else
                    Dr = Ds.Tables(0).Rows(0)
                    DendaAsisten.Text = Dr("Emp_No").ToString
                    DendaAsistenName.Text = Dr("Emp_Name").ToString
                End If

            Next
        Catch ex As Exception
            lbstatus.Text = "DendaAsisten_OnTextChanged Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DendaMandor_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim GVR As GridViewRow
        Dim Ds As New DataSet()
        Dim DendaMandor, DendaMandorName As TextBox
        Dim Dr As DataRow
        Try
            For Each GVR In DataGridDenda.Rows
                DendaMandor = GVR.FindControl("DendaMandor")
                DendaMandorName = GVR.FindControl("DendaMandorName")
                Ds = SQLExecuteQuery("SELECT Emp_No, Emp_Name FROM V_MsEmployee WHERE Emp_no = " + QuotedStr(DendaMandor.Text), ViewState("DBConnection").ToString)

                If IsNothing(Ds) Or Ds.Tables(0).Rows.Count <= 0 Or IsNothing(Ds.Tables(0)) Then
                    DendaMandor.Text = ""
                    DendaMandorName.Text = ""
                Else
                    Dr = Ds.Tables(0).Rows(0)
                    DendaMandor.Text = Dr("Emp_No").ToString
                    DendaMandorName.Text = Dr("Emp_Name").ToString
                End If

            Next
        Catch ex As Exception
            lbstatus.Text = "DendaMandor_OnTextChanged Changed Error : " + ex.ToString
        End Try
    End Sub

    Private Sub bindDataProcessAudit()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbTrans, lbWONO, lbtype, lbBlock, lbQtyLKM, lbprice, lbbruto, lbnetto As Label
            Dim tbQty, tbdisc, tbdiscforex As TextBox
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGridAudit.Rows
                CB = GVR.FindControl("cbSelect")
                lbTrans = GVR.FindControl("TransNmbr")
                lbWONO = GVR.FindControl("WONo")
                lbtype = GVR.FindControl("Type")
                lbBlock = GVR.FindControl("DivisiBlock")
                lbQtyLKM = GVR.FindControl("QtyLKM")
                lbprice = GVR.FindControl("AuditPrice")
                lbbruto = GVR.FindControl("AuditBruto")
                lbnetto = GVR.FindControl("AuditNetto")
                tbQty = GVR.FindControl("AuditQty")
                tbdisc = GVR.FindControl("AuditDiscPercent")
                tbdiscforex = GVR.FindControl("AuditDisc")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOResultCompleteAudit " + QuotedStr(lbTrans.Text) + _
                    ", " + QuotedStr(lbWONO.Text) + ", " + QuotedStr(lbtype.Text) + ", " + QuotedStr(lbBlock.Text) + ", " + QuotedStr(ViewState("ServerDate")) + ", " + QuotedStr(lbQtyLKM.Text.Replace(",", "")) + ", " + QuotedStr(tbQty.Text.Replace(",", "")) + _
                    ", " + QuotedStr(lbprice.Text.Replace(",", "")) + ", " + QuotedStr(lbbruto.Text.Replace(",", "")) + ", " + QuotedStr(tbdisc.Text) + ", " + QuotedStr(tbdiscforex.Text.Replace(",", "")) + ", " + QuotedStr(lbnetto.Text.Replace(",", "")) + _
                    ", " + QuotedStr(tbPIC.Text) + ", " + QuotedStr(tbRemark1.Text) + ", " + QuotedStr(tbRemark2.Text) + ", " + QuotedStr(tbRemark3.Text) + _
                    ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If
                End If
            Next
            BindDataAudit()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check Week for Process Premi"
                Exit Sub
            Else
                lbstatus.Text = "Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub




    Private Sub bindDataProcessDenda()
        Try
            Dim GVR As GridViewRow
            Dim CB As CheckBox
            Dim lbTrans, lbWONO, lbtype, lbBlock As Label
            Dim lbDendaAmount, lbDendaManager, lbDendaManagerPercent, lbDendaAsisten, tbDendaAsistenPercent, tbDendaAskep, tbDendaAskepPercent, tbDendaMandor, tbDendaMandorPercent As TextBox
            Dim HaveSelect As Boolean

            Dim SQLString, Hasil As String
            HaveSelect = False
            For Each GVR In DataGridDenda.Rows
                CB = GVR.FindControl("cbSelect")
                lbTrans = GVR.FindControl("TransNmbr")
                lbWONO = GVR.FindControl("WONo")
                lbtype = GVR.FindControl("Type")
                lbBlock = GVR.FindControl("DivisiBlock")
                lbDendaAmount = GVR.FindControl("DendaAmount")
                lbDendaManager = GVR.FindControl("DendaManager")
                lbDendaManagerPercent = GVR.FindControl("DendaManagerPercent")
                lbDendaAsisten = GVR.FindControl("DendaAsisten")
                tbDendaAsistenPercent = GVR.FindControl("DendaAsistenPercent")
                tbDendaAskep = GVR.FindControl("DendaAskep")
                tbDendaAskepPercent = GVR.FindControl("DendaAskepPercent")
                tbDendaMandor = GVR.FindControl("DendaMandor")
                tbDendaMandorPercent = GVR.FindControl("DendaMandorPercent")

                If CB.Checked Then
                    HaveSelect = True
                    SQLString = "DECLARE @A VARCHAR(255) EXEC S_PLWOResultCompleteDenda " + QuotedStr(lbTrans.Text) + _
                    ", " + QuotedStr(lbWONO.Text) + ", " + QuotedStr(lbtype.Text) + ", " + QuotedStr(lbBlock.Text) + ", " + QuotedStr(ViewState("ServerDate")) + ", " + QuotedStr(lbDendaAmount.Text.Replace(",", "")) + _
                    ", " + QuotedStr(lbDendaManager.Text) + ", " + QuotedStr(lbDendaManagerPercent.Text) + ", " + QuotedStr(tbDendaAskep.Text) + ", " + QuotedStr(tbDendaAskepPercent.Text.Replace(",", "")) + ", " + QuotedStr(lbDendaAsisten.Text) + _
                    ", " + tbDendaAsistenPercent.Text + ", " + QuotedStr(tbDendaMandor.Text) + ", " + tbDendaMandorPercent.Text + _
                    ", " + QuotedStr(tbRemark1.Text) + _
                    ", " + ViewState("GLYear").ToString + ", " + ViewState("GLPeriod").ToString + _
                    ", " + QuotedStr(ViewState("UserId").ToString) + ", @A OUT SELECT @A "
                    SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
                    Hasil = SQLExecuteScalar(SQLString, ViewState("DBConnection").ToString)
                    Hasil = Replace(Hasil, "0", "")

                    If Trim(Hasil) <> "" Then
                        lbstatus.Text = MessageDlg(Hasil)
                        ' Exit For
                    End If
                End If
            Next
            BindDataDenda()
            If HaveSelect = False Then
                lbstatus.Text = "Please Check Week for Process Premi"
                Exit Sub
            Else
                lbstatus.Text = "Process Success "
            End If

        Catch ex As Exception
            Throw New Exception("bindDataSetLF Error : " + ex.ToString)
        End Try
    End Sub

    Protected Sub DataGridDenda_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridDenda.PageIndexChanging
        DataGridDenda.PageIndex = e.NewPageIndex
        BindDataDenda()
    End Sub

    Protected Sub DataGridAudit_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DataGridAudit.PageIndexChanging
        DataGridAudit.PageIndex = e.NewPageIndex
        BindDataAudit()
    End Sub
End Class