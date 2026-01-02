
Partial Class UserControl_AdvanceFind
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(Request.QueryString("KeyId")) Is Nothing Then
        ' lbStatus.text = MessageDlg("Sesi anda telah habis silahkan login kembali")
            Response.Redirect("~\Sesi.aspx")
        End If
        Dim Len, i As Integer
        Dim FieldName(), FieldValue() As String
        Try
            If Not IsPostBack Then
                FieldName = Session("DateFieldName")
                FieldValue = Session("DateFieldValue")
                Len = FieldName.Length
                For i = 0 To Len - 1
                    Dim LI As New ListItem(FieldName(i).Trim, FieldValue(i).Trim)
                    ddlDateField1.Items.Add(LI)
                    ddlDateField2.Items.Add(LI)
                Next

                FieldName = Session("FieldName")
                FieldValue = Session("FieldValue")
                Len = FieldName.Length
                For i = 0 To Len - 1
                    Dim LI As New ListItem(FieldName(i).Trim, FieldValue(i).Trim)
                    ddlField1.Items.Add(LI)
                    ddlField2.Items.Add(LI)
                    ddlField3.Items.Add(LI)
                Next
                Session("DateFieldName") = Nothing
                Session("DateFieldValue") = Nothing
                Session("FieldName") = Nothing
                Session("FieldValue") = Nothing
            End If
        Catch ex As Exception
            lbStatus.Text = "page load error : " + ex.ToString
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim sqlFilter, fieldStr As String
        Dim pertamax As Boolean
        Try
            sqlFilter = ""
            pertamax = True
            If ddlOperatorDate1.SelectedValue <> "-" Then
                If pertamax Then
                    sqlFilter = " (" + ddlDateField1.SelectedValue + " BETWEEN " + QuotedStr(Format(tbStartDate1.Date, "dd MMM yyyy")) + " AND " + QuotedStr(Format(tbEndDate1.Date, "dd MMM yyyy")) + ")"
                    pertamax = False
                Else
                    sqlFilter = ddlOperatorDate1.SelectedValue + " (" + ddlDateField1.SelectedValue + " BETWEEN " + QuotedStr(Format(tbStartDate1.Date, "dd MMM yyyy")) + " AND " + QuotedStr(Format(tbEndDate1.Date, "dd MMM yyyy")) + ")"
                End If
            End If

            If ddlOperatorDate2.SelectedValue <> "-" Then
                If pertamax Then
                    sqlFilter = sqlFilter + " (" + ddlDateField2.SelectedValue + " BETWEEN " + QuotedStr(Format(tbStartDate2.Date, "dd MMM yyyy")) + " AND " + QuotedStr(Format(tbEndDate2.Date, "dd MMM yyyy")) + ")"
                    pertamax = False
                Else
                    sqlFilter = sqlFilter + " " + ddlOperatorDate2.SelectedValue + " (" + ddlDateField2.SelectedValue + " BETWEEN " + QuotedStr(Format(tbStartDate2.Date, "dd MMM yyyy")) + " AND " + QuotedStr(Format(tbEndDate2.Date, "dd MMM yyyy")) + ")"
                End If

            End If

            If tbField1.Text.Length > 0 And ddlOperator1.SelectedValue <> "-" Then
                If ddlNotasi1.SelectedIndex >= 3 Then
                    fieldStr = ddlNotasi1.SelectedValue + " '%" + tbField1.Text.Replace("'", "''") + "%'"
                Else
                    fieldStr = ddlNotasi1.SelectedValue + " " + QuotedStr(tbField1.Text)
                End If
                If pertamax Then
                    sqlFilter = sqlFilter + " (" + ddlField1.SelectedValue + " " + fieldStr + ")"
                    pertamax = False
                Else
                    sqlFilter = sqlFilter + " " + ddlOperator1.SelectedValue + " (" + ddlField1.SelectedValue + " " + fieldStr + ")"
                End If

            End If

            If tbField2.Text.Length > 0 And ddlOperator2.SelectedValue <> "-" Then
                If ddlNotasi2.SelectedIndex >= 3 Then
                    fieldStr = ddlNotasi2.SelectedValue + " '%" + tbField2.Text.Replace("'", "''") + "%'"
                Else
                    fieldStr = ddlNotasi2.SelectedValue + " " + QuotedStr(tbField2.Text)
                End If
                If pertamax Then
                    sqlFilter = sqlFilter + " (" + ddlField2.SelectedValue + " " + fieldStr + ")"
                    pertamax = False
                Else
                    sqlFilter = sqlFilter + " " + ddlOperator2.SelectedValue + " (" + ddlField2.SelectedValue + " " + fieldStr + ")"
                End If

            End If

            If tbField3.Text.Length > 0 And ddlOperator3.SelectedValue <> "-" Then
                If ddlNotasi3.SelectedIndex >= 3 Then
                    fieldStr = ddlNotasi3.SelectedValue + " '%" + tbField3.Text.Replace("'", "''") + "%'"
                Else
                    fieldStr = ddlNotasi3.SelectedValue + " " + QuotedStr(tbField3.Text)
                End If
                If pertamax Then
                    sqlFilter = sqlFilter + " (" + ddlField3.SelectedValue + " " + fieldStr + ")"
                    pertamax = False
                Else
                    sqlFilter = sqlFilter + " " + ddlOperator3.SelectedValue + " (" + ddlField3.SelectedValue + " " + fieldStr + ")"
                End If
            End If

            Session("AdvanceFilter") = sqlFilter
            Session("FgAdvanceFilter") = "Y"
            Response.Write("<script language='javascript'> { window.opener.document.form1.submit();  window.close();}</script>")
        Catch ex As Exception
            lbStatus.Text = "btn Search error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlOperatorDate1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOperatorDate1.SelectedIndexChanged
        Try
            tbStartDate1.Enabled = ddlOperatorDate1.SelectedValue <> "-"
            tbEndDate1.Enabled = ddlOperatorDate1.SelectedValue <> "-"
            ddlDateField1.Enabled = ddlOperatorDate1.SelectedValue <> "-"
        Catch ex As Exception
            lbStatus.Text = "ddl operator date 1 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlOperatorDate2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOperatorDate2.SelectedIndexChanged
        Try
            tbStartDate2.Enabled = ddlOperatorDate2.SelectedValue <> "-"
            tbEndDate2.Enabled = ddlOperatorDate2.SelectedValue <> "-"
            ddlDateField2.Enabled = ddlOperatorDate2.SelectedValue <> "-"
        Catch ex As Exception
            lbStatus.Text = "ddl operator date 2 Error : " + ex.ToString
        End Try
    End Sub

    Protected Sub ddlOperator1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOperator1.SelectedIndexChanged
        Try
            ddlField1.Enabled = ddlOperator1.SelectedValue <> "-"
            ddlNotasi1.Enabled = ddlOperator1.SelectedValue <> "-"
            tbField1.Enabled = ddlOperator1.SelectedValue <> "-"
        Catch ex As Exception
            lbStatus.Text = "ddlOperator 1 Error :" + ex.ToString
        End Try
    End Sub

    Protected Sub ddlOperator2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOperator2.SelectedIndexChanged
        Try
            ddlField2.Enabled = ddlOperator2.SelectedValue <> "-"
            ddlNotasi2.Enabled = ddlOperator2.SelectedValue <> "-"
            tbField2.Enabled = ddlOperator2.SelectedValue <> "-"
        Catch ex As Exception
            lbStatus.Text = "ddlOperator 2 Error :" + ex.ToString
        End Try
    End Sub

    Protected Sub ddlOperator3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOperator3.SelectedIndexChanged
        Try
            ddlField3.Enabled = ddlOperator3.SelectedValue <> "-"
            ddlNotasi3.Enabled = ddlOperator3.SelectedValue <> "-"
            tbField3.Enabled = ddlOperator3.SelectedValue <> "-"
        Catch ex As Exception
            lbStatus.Text = "ddlOperator 3 Error :" + ex.ToString
        End Try
    End Sub
End Class
