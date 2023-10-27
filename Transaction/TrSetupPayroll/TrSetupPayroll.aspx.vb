Imports System.Data
Partial Class Transaction_TrSetupPayroll_TrSetupPayroll
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitProperty()
            ViewState("SortExpression") = Nothing
            bindDataGrid()
        End If
        If Not Session("Result") Is Nothing Then
            If ViewState("Sender") = "btnSearchPayroll" Then
                tbPayroll.Text = Session("Result")(0).ToString
                tbPayrollName.Text = Session("Result")(1).ToString
            End If
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

    Private Sub bindDataGrid()
        Dim tempDS As New DataSet()
        Dim DV As DataView
        Try
            tempDS = SQLExecuteQuery("SELECT A.SetGroup, A.SetCode, A.SetDescription, A.SetValue, COALESCE(A.SetRemark, '') AS SetRemark, " + _
                                     " B.PayrollName FROM MsSetup A LEFT OUTER JOIN MsPayroll B ON A.SetValue = B.PayrollCode " + _
                                     " WHERE SetGroup = 'Payroll' ORDER BY A.SetCode", ViewState("DBConnection").ToString)

            DV = tempDS.Tables(0).DefaultView

            DV.Sort = ViewState("SortExpression")
            DataGrid.DataSource = DV
            DataGrid.DataBind()

        Catch ex As Exception
            lstatus.Text = lstatus.Text + "BindDataGrid Error: " & ex.ToString
        Finally
        End Try
    End Sub

    Public Sub ItemCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Try
            If e.CommandName = "Edit" Then
                PnlMain.Visible = False
                pnlInput.Visible = True
                Dim lbDesc, lbCode, lbValue, lbRemark, lbName As Label
                lbDesc = e.Item.FindControl("SetDescription")
                lbCode = e.Item.FindControl("SetCode")
                lbName = e.Item.FindControl("PayrollName")
                lbValue = e.Item.FindControl("SetValue")
                lbRemark = e.Item.FindControl("SetRemark")

                lbSetDescription.Text = lbDesc.Text
                tbPayroll.Text = lbValue.Text
                tbPayrollName.Text = lbName.Text
                ViewState("SetRemark") = lbRemark.Text.Replace("***", ViewState("Currency"))
                ViewState("SetCode") = lbCode.Text
            ElseIf e.CommandName = "Delete" Then
                Dim lbCode As Label
                lbCode = e.Item.FindControl("SetCode")
                SQLExecuteNonQuery("UPDATE MsSetup SET SetValue=NULL WHERE SetGroup = 'Payroll' AND SetCode =" + QuotedStr(lbCode.Text), ViewState("DBConnection").ToString)
                bindDataGrid()
            End If
        Catch ex As Exception
            lstatus.Text = lstatus.Text + "InsertCommand Error:" & ex.ToString
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            pnlInput.Visible = False
            PnlMain.Visible = True
            bindDataGrid()
        Catch ex As Exception
            lstatus.Text = "btn Cancel Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            tbPayroll.Text = ""
            tbPayrollName.Text = ""
        Catch ex As Exception
            lstatus.Text = "btn Reset Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim SQLString As String = ""
        Try
            If tbPayroll.Text.Trim = "" Then
                lstatus.Text = "Payroll Must Have Value."
                tbPayroll.Focus()
                Exit Sub
            End If

            SQLString = "Update MsSetup set SetValue= " + QuotedStr(tbPayroll.Text) + " WHERE SetGroup = 'Payroll' AND SetCode =" + QuotedStr(ViewState("SetCode"))

            SQLExecuteNonQuery(SQLString, ViewState("DBConnection").ToString)
            bindDataGrid()
            pnlInput.Visible = False
            PnlMain.Visible = True
        Catch ex As Exception
            lstatus.Text = "btn Save Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearchPayroll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchPayroll.Click
        Dim Filter(10) As String
        Dim Remark, ResultField As String
        Try
            If ViewState("SetRemark").length > 0 Then
                Remark = "Where " + ViewState("SetRemark")
            Else
                Remark = ""
            End If
            Session("filter") = "SELECT Payroll_Code, Payroll_Name From V_MsPayroll " + Remark
            ResultField = "Payroll_Code, Payroll_Name"
            Session("Column") = ResultField.Split(",")
            Session("DBConnection") = ViewState("DBConnection")
            ViewState("Sender") = "btnSearchPayroll"
            If (Not ClientScript.IsStartupScriptRegistered("tes")) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "tes", "OpenPopup();", True)
            End If
        Catch ex As Exception
            lstatus.Text = "btn Search Payroll Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub tbPayroll_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPayroll.TextChanged
        Dim dr As DataRow
        Dim ds As DataSet
        Dim Remark As String
        Try
            If ViewState("SetRemark").length > 0 Then
                Remark = "WHERE " + ViewState("SetRemark") + " AND Payroll_Code = " + QuotedStr(tbPayroll.Text)
            Else
                Remark = "WHERE Payroll_Code = " + QuotedStr(tbPayroll.Text)
            End If
            ds = SQLExecuteQuery("SELECT Payroll_Code, Payroll_Name FROM V_MsPayroll " + Remark, ViewState("DBConnection").ToString)
            If IsNothing(ds) Or ds.Tables(0).Rows.Count <= 0 Or IsNothing(ds.Tables(0)) Then
                tbPayroll.Text = ""
                tbPayrollName.Text = ""
            Else
                dr = ds.Tables(0).Rows(0)
                tbPayroll.Text = dr("Payroll_Code").ToString
                tbPayrollName.Text = dr("Payroll_Name").ToString
            End If

        Catch ex As Exception
            lstatus.Text = "tb Payroll Changed Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_BeforeColumnSortingGrouping(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs) Handles DataGrid.BeforeColumnSortingGrouping
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid.PageIndexChanged
        bindDataGrid()
    End Sub
    Protected Sub DataGrid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs) Handles DataGrid.ProcessColumnAutoFilter
        bindDataGrid()
    End Sub

    Protected Sub DataGrid_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs) Handles DataGrid.CustomCallback
        Dim SelectValue As Object
        Dim val(4) As String
        Try
            val(0) = "SetDescription"
            val(1) = "SetValue"
            val(2) = "PayrollName"
            val(3) = "SetRemark"
            val(4) = "SetCode"
            SelectValue = DataGrid.GetRowValues(Convert.ToInt32(e.Parameters), val)

            PnlMain.Visible = False
            pnlInput.Visible = True

            lbSetDescription.Text = SelectValue(0).ToString
            tbPayroll.Text = SelectValue(1).ToString
            tbPayrollName.Text = SelectValue(2).ToString
            ViewState("SetRemark") = SelectValue(3).ToString.Replace("***", ViewState("Currency"))
            ViewState("SetCode") = SelectValue(4).ToString
        Catch ex As Exception
            lstatus.Text = "Data Grid Custom Call Back Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub DataGrid_CustomButtonCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs) Handles DataGrid.CustomButtonCallback
        Dim SelectValue As Object
        Dim i As Integer
        Dim kod As String
        Try
            SelectValue = DataGrid.GetRowValues(DataGrid.FocusedRowIndex, "SetCode")
            kod = ""
            For i = 0 To SelectValue.length - 1
                kod = kod + SelectValue(i).ToString
            Next
            SQLExecuteNonQuery("UPDATE MsSetup SET SetValue=NULL WHERE SetGroup = 'Payroll' AND SetCode =" + QuotedStr(kod), ViewState("DBConnection").ToString)
            bindDataGrid()

        Catch ex As Exception
            lstatus.Text = "Custom Button Call Back Error : " + ex.ToString
        End Try
    End Sub
End Class
