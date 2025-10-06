Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Web.UI.ClientScriptManager
Imports BasicFrame.WebControls

Public Module Utilities

    Public Sub FillAction(ByVal  btn1 As WebControl, ByVal btn2 As WebControl, ByRef dd1 As DropDownList, ByRef dd2 As DropDownList, ByVal data As DataRow)
        Try
            btn1.Visible = data("FgInsert") = "Y"
            btn2.Visible = btn1.Visible

            If data("FgGetAppr") = "Y" Then
                dd1.Items.Add("Get Approval")
                dd2.Items.Add("Get Approval")
            End If

            If data("FgPost") = "Y" Then
                dd1.Items.Add("Post")
                dd2.Items.Add("Post")
            End If

            If data("FgUnPost") = "Y" Then
                dd1.Items.Add("Un-Post")
                dd2.Items.Add("Un-Post")
            End If
            'If data("FgPrint") = "N" Then
            '    Dim removeListItem As ListItem = dd1.Items.FindByValue("Print")
            '    If dd1.Items.Contains(removeListItem) Then
            '        dd1.Items.Remove(removeListItem)
            '        dd2.Items.Remove(removeListItem)
            '    End If
            'End If
            If data("FgComplete") = "Y" Then
                dd1.Items.Add("Complete")
                dd2.Items.Add("Complete")
            End If

            If data("FgDelete") = "Y" Then
                dd1.Items.Add("Delete")
                dd2.Items.Add("Delete")
            End If

            If dd1.Items.Count = 0 Then
                dd1.Items.Add("No Data")
                dd2.Items.Add("No Data")
            End If
        Catch ex As Exception
            Throw New Exception("Fill Action Error : " + ex.ToString)
        End Try
    End Sub

    

    Public Function CheckMenuLevel(ByVal CommandName As String, ByVal Dr As DataRow) As String
        Try
            If CommandName = "Insert" Then
                If Dr("FgInsert") = "N" Then
                    Return MessageDlg("You are not authorized to insert record. Please contact administrator")
                    Exit Function
                End If
            End If
            If CommandName = "Edit" Then
                If Dr("FgEdit") = "N" Then
                    Return MessageDlg("You are not authorized to edit record. Please contact administrator")
                    Exit Function
                End If
            End If
            If CommandName = "Delete" Then
                If Dr("FgDelete") = "N" Then
                    Return MessageDlg("You are not authorized to delete record. Please contact administrator")
                End If
            End If
            If CommandName = "Complete" Then
                If Dr("FgComplete") = "N" Then
                    Return MessageDlg("You are not authorized to complete record. Please contact administrator")
                    Exit Function
                End If
            End If
            If CommandName = "Un-Complete" Or CommandName = "Un-Post" Then
                If Dr("FgUnpost") = "N" Then
                    Return MessageDlg("You are not authorized to undo record. Please contact administrator")
                    Exit Function
                End If
            End If
            If CommandName = "Print" Then
                If Dr("FgPrint") = "N" Then
                    Return MessageDlg("You are not authorized to Print record. Please contact administrator")
                End If
            End If
            Return ""
        Catch ex As Exception
            Throw New Exception("Check Menu Level Error : " + ex.ToString)
        End Try
    End Function

    Public Sub AttachScript(ByVal FunctionName As String, ByRef myPage As System.Web.UI.Page, ByVal TipeSaya As System.Type)
        Try
            If (Not myPage.ClientScript.IsStartupScriptRegistered("tes")) Then
                myPage.ClientScript.RegisterStartupScript(TipeSaya, "tes", FunctionName, True)
            End If
        Catch ex As Exception
            Throw New Exception(" Attach Script Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub AttachScriptAJAX(ByVal FunctionName As String, ByRef myPage As System.Web.UI.Page, ByVal TipeSaya As System.Type)
        Try
            'If (Not myPage.ClientScript.IsStartupScriptRegistered("tes")) Then
            'myPage.ClientScript.RegisterStartupScript(TipeSaya, "tes", FunctionName, True)
            ScriptManager.RegisterStartupScript(myPage, TipeSaya, Guid.NewGuid().ToString(), FunctionName, True)
            'End If
        Catch ex As Exception
            Throw New Exception(" Attach Script AJAX Error : " + ex.ToString)
        End Try
    End Sub



    Public Function CekStatus(ByVal Value As String) As String
        Select Case Value
            Case "Get Approval"
                Return "H"
            Case "Post"
                Return "G"
            Case "Un-Post"
                Return "P"
            Case "Delete"
                Return "H,G"
            Case "Cancel"
                Return "P"
            Case "Complete"
                Return "P"
            Case Else
                Return "Exit"
        End Select
    End Function

    Public Sub GetListCommand(ByVal CekStatus As String, ByVal Grid As GridView, ByVal ColStatusWithPrimaryKey As String, ByRef ListSelectNmbr As String, ByRef Nmbr() As String, ByRef EMsg As String, Optional ByVal StrSplitColumn As String = "")
        Dim DGI As GridViewRow
        Dim Cb As CheckBox
        Dim FirstTime As Boolean
        Dim i As Integer
        Dim FieldKey() As String
        Dim StatusKey, PrimaryKey, StrPrimaryKey As String
        Try
            i = 0
            ListSelectNmbr = ""
            FieldKey = ColStatusWithPrimaryKey.Split(",")
            PrimaryKey = ""
            StrPrimaryKey = ""
            EMsg = ""
            FirstTime = True
            For Each DGI In Grid.Rows
                Cb = DGI.FindControl("cbSelect")
                If Cb.Checked Then
                    StatusKey = DGI.Cells(CInt(FieldKey(0))).Text
                    If FieldKey.Length <= 2 Then
                        PrimaryKey = DGI.Cells(CInt(FieldKey(1))).Text
                        StrPrimaryKey = QuotedStr(PrimaryKey)
                    ElseIf FieldKey.Length = 3 Then
                        PrimaryKey = DGI.Cells(CInt(FieldKey(1))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(2))).Text.Trim
                        StrPrimaryKey = QuotedStr(DGI.Cells(CInt(FieldKey(1))).Text) + " " + StrSplitColumn + " " + QuotedStr(DGI.Cells(CInt(FieldKey(2))).Text)
                    ElseIf FieldKey.Length = 4 Then
                        PrimaryKey = DGI.Cells(CInt(FieldKey(1))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(2))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(3))).Text.Trim
                        StrPrimaryKey = QuotedStr(DGI.Cells(CInt(FieldKey(1))).Text) + " " + StrSplitColumn + " " + QuotedStr(DGI.Cells(CInt(FieldKey(2))).Text) + " " + QuotedStr(DGI.Cells(CInt(FieldKey(3))).Text)
                    ElseIf FieldKey.Length = 5 Then
                        PrimaryKey = DGI.Cells(CInt(FieldKey(1))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(2))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(3))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(4))).Text.Trim
                        StrPrimaryKey = QuotedStr(DGI.Cells(CInt(FieldKey(1))).Text) + " " + StrSplitColumn + " " + QuotedStr(DGI.Cells(CInt(FieldKey(2))).Text) + " " + QuotedStr(DGI.Cells(CInt(FieldKey(3))).Text) + " " + QuotedStr(DGI.Cells(CInt(FieldKey(4))).Text)
                    ElseIf FieldKey.Length = 6 Then
                        PrimaryKey = DGI.Cells(CInt(FieldKey(1))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(2))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(3))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(4))).Text.Trim + "|" + DGI.Cells(CInt(FieldKey(5))).Text.Trim
                        StrPrimaryKey = QuotedStr(DGI.Cells(CInt(FieldKey(1))).Text) + " " + StrSplitColumn + " " + QuotedStr(DGI.Cells(CInt(FieldKey(2))).Text) + " " + QuotedStr(DGI.Cells(CInt(FieldKey(3))).Text) + " " + QuotedStr(DGI.Cells(CInt(FieldKey(4))).Text) + " " + QuotedStr(DGI.Cells(CInt(FieldKey(5))).Text)
                    End If
                    If Not CekStatus.Contains(StatusKey) Then
                        EMsg = EMsg + "Status " + StrPrimaryKey + " must be " + CekStatus + " <br/>"
                    Else
                        If PrimaryKey.Trim <> "" Then
                            Nmbr(i) = PrimaryKey
                            i = i + 1
                        End If
                    End If
                    If FirstTime Then
                        FirstTime = False
                        ListSelectNmbr = QuotedStr(PrimaryKey)
                    Else
                        ListSelectNmbr = ListSelectNmbr + "," + QuotedStr(PrimaryKey)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception("Go Command Click Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub ModifyInput(ByVal State As Boolean, ByVal pnl As Panel)
        Try
            Dim I, count As Integer
            count = pnl.Controls.Count - 1
            I = 1
            While I < count
                If TypeOf (pnl.Controls(I)) Is TextBox Then
                    Dim Text As TextBox
                    Text = pnl.Controls(I)
                    If Text.ValidationGroup = "Input" Then
                        Text.Enabled = State
                    End If
                ElseIf TypeOf (pnl.Controls(I)) Is DropDownList Then
                    Dim Ddl As DropDownList
                    Ddl = pnl.Controls(I)
                    If Ddl.ValidationGroup = "Input" Then
                        Ddl.Enabled = State
                    End If
                ElseIf TypeOf (pnl.Controls(I)) Is BasicDatePicker Then
                    Dim TextDt As BasicDatePicker
                    TextDt = pnl.Controls(I)
                    If TextDt.ValidationGroup = "Input" Then
                        TextDt.Enabled = State
                    End If
                ElseIf TypeOf (pnl.Controls(I)) Is ImageButton Then
                    Dim Ibtn As ImageButton
                    Ibtn = pnl.Controls(I)
                    If Ibtn.ValidationGroup = "Input" Then
                        Ibtn.Visible = State
                    End If
                ElseIf TypeOf (pnl.Controls(I)) Is LinkButton Then
                    Dim Lkb As LinkButton
                    Lkb = pnl.Controls(I)
                    If Lkb.ValidationGroup = "Input" Then
                        Lkb.Enabled = State
                    End If
                ElseIf TypeOf (pnl.Controls(I)) Is Button Then
                    Dim btn As Button
                    btn = pnl.Controls(I)
                    If btn.ValidationGroup = "Input" Then
                        btn.Visible = State
                    End If
                End If
                I = I + 1
            End While
        Catch ex As Exception
            Throw New Exception("Modify Input Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub ModifyInput2(ByVal State As Boolean, ByVal pnl1 As Panel, ByVal pnl2 As Panel, ByVal GridDt As GridView)
        Try
            Dim I, count As Integer
            count = pnl1.Controls.Count - 1
            I = 1
            While I < count
                If TypeOf (pnl1.Controls(I)) Is TextBox Then
                    Dim Text As TextBox
                    Text = pnl1.Controls(I)
                    If Text.ValidationGroup = "Input" Then
                        Text.Enabled = State
                    End If
                ElseIf TypeOf (pnl1.Controls(I)) Is DropDownList Then
                    Dim Ddl As DropDownList
                    Ddl = pnl1.Controls(I)
                    If Ddl.ValidationGroup = "Input" Then
                        Ddl.Enabled = State
                    End If
                ElseIf TypeOf (pnl1.Controls(I)) Is BasicDatePicker Then
                    Dim TextDt As BasicDatePicker
                    TextDt = pnl1.Controls(I)
                    If TextDt.ValidationGroup = "Input" Then
                        TextDt.Enabled = State
                    End If
                ElseIf TypeOf (pnl1.Controls(I)) Is ImageButton Then
                    Dim Ibtn As ImageButton
                    Ibtn = pnl1.Controls(I)
                    If Ibtn.ValidationGroup = "Input" Then
                        Ibtn.Visible = State
                    End If
                ElseIf TypeOf (pnl1.Controls(I)) Is LinkButton Then
                    Dim Lkb As LinkButton
                    Lkb = pnl1.Controls(I)
                    If Lkb.ValidationGroup = "Input" Then
                        Lkb.Enabled = State
                    End If
                ElseIf TypeOf (pnl1.Controls(I)) Is Button Then
                    Dim btn As Button
                    btn = pnl1.Controls(I)
                    If btn.ValidationGroup = "Input" Then
                        btn.Visible = State
                    End If
                End If
                I = I + 1
            End While

            count = pnl2.Controls.Count - 1
            I = 1
            While I < count
                If TypeOf (pnl2.Controls(I)) Is TextBox Then
                    Dim Text As TextBox
                    Text = pnl2.Controls(I)
                    If Text.ValidationGroup = "Input" Then
                        Text.Enabled = State
                    End If
                ElseIf TypeOf (pnl2.Controls(I)) Is DropDownList Then
                    Dim Ddl As DropDownList
                    Ddl = pnl2.Controls(I)
                    If Ddl.ValidationGroup = "Input" Then
                        Ddl.Enabled = State
                    End If
                ElseIf TypeOf (pnl2.Controls(I)) Is BasicDatePicker Then
                    Dim TextDt As BasicDatePicker
                    TextDt = pnl2.Controls(I)
                    If TextDt.ValidationGroup = "Input" Then
                        TextDt.Enabled = State
                    End If
                ElseIf TypeOf (pnl2.Controls(I)) Is ImageButton Then
                    Dim Ibtn As ImageButton
                    Ibtn = pnl2.Controls(I)
                    If Ibtn.ValidationGroup = "Input" Then
                        Ibtn.Visible = State
                    End If
                ElseIf TypeOf (pnl2.Controls(I)) Is LinkButton Then
                    Dim Lkb As LinkButton
                    Lkb = pnl2.Controls(I)
                    If Lkb.ValidationGroup = "Input" Then
                        Lkb.Enabled = State
                    End If
                ElseIf TypeOf (pnl2.Controls(I)) Is Button Then
                    Dim btn As Button
                    btn = pnl2.Controls(I)
                    If btn.ValidationGroup = "Input" Then
                        btn.Visible = State
                    End If
                End If
                I = I + 1
            End While
            GridDt.Columns(0).Visible = State
        Catch ex As Exception
            Throw New Exception("Modify Input Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub ChangeReport(ByVal State As String, ByVal FgReport As String, ByVal HomeCurrency As Boolean, ByVal txdate As BasicDatePicker, ByVal txRate As TextBox, Optional ByRef txPPnNo As TextBox = Nothing, Optional ByRef txppndate As BasicDatePicker = Nothing, Optional ByRef TxPPnRate As TextBox = Nothing)
        Try
            txPPnNo.Enabled = FgReport = "Y"
            txppndate.Enabled = FgReport = "Y"
            TxPPnRate.Enabled = FgReport = "Y" And (Not HomeCurrency)
            If State.ToUpper = "ADD" Or State.ToUpper = "EDIT" Then
                If FgReport = "N" Then
                    txPPnNo.Text = ""
                    txppndate.Clear()
                    TxPPnRate.Text = "0"
                Else
                    txppndate.SelectedValue = txdate.SelectedValue
                    TxPPnRate.Text = txRate.Text
                End If
            End If

        Catch ex As Exception
            Throw New Exception("Report Change Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub MovePanel(ByRef FromPanel As Panel, ByRef ToPanel As Panel)
        FromPanel.Visible = False
        ToPanel.Visible = True
    End Sub

    Public Sub ChangeCurrency(ByVal ddl As DropDownList, ByVal tbdate As BasicDatePicker, ByRef txrate As TextBox, ByVal HomeCurrency As String, ByRef DigitCurr As String, ByVal DBConnection As String, Optional ByVal DigitHome As String = "0", Optional ByVal DigitRate As String = "0")
        Dim Dr As DataRow
        Try
            txrate.Enabled = (ddl.SelectedValue <> HomeCurrency) And (ddl.SelectedValue <> "")
            If txrate.Enabled = True Then
                Dr = FindMaster("Rate", ddl.SelectedValue + "|" + Format(tbdate.SelectedDate, "yyyy-MM-dd"), DBConnection)
                If Not Dr Is Nothing Then
                    txrate.Text = FormatFloat(Dr("Rate").ToString, DigitRate)
                    DigitCurr = Dr("digit")
                Else
                    txrate.Text = FormatFloat("0", DigitRate)
                End If
            Else
                txrate.Text = FormatFloat("1", DigitRate)
                DigitCurr = DigitHome
            End If
        Catch ex As Exception
            Throw New Exception("Change Currency Error : " + ex.ToString)
        End Try
    End Sub

    Public Function GetCurrRate(ByVal currency As String, ByVal tgl As DateTime, ByVal DBConnection As String) As Double
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Rate", currency + "|" + Format(tgl, "yyyy-MM-dd"), DBConnection)
            If Not Dr Is Nothing Then
                Return Dr("Rate")
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception("GetCurrRate Error : " + ex.ToString)
        End Try
    End Function

    Public Function GetCurrDigit(ByVal currency As String, ByVal DBConnection As String) As Integer
        Dim Dr As DataRow
        Try
            Dr = FindMaster("Decimal", currency, DBConnection)
            If Not Dr Is Nothing Then
                Return Dr("Digit")
            End If
            Return 0
        Catch ex As Exception
            Throw New Exception("GetCurrDigit Error : " + ex.ToString)
        End Try
    End Function

    Public Sub RefreshMaster(ByVal SpName As String, ByVal SelectValue As String, ByVal SelectName As String, ByRef ddl As DropDownList, Optional ByVal DBConnection As String = "Nothing")
        Dim LI As ListItem
        Try
            LI = ddl.Items(ddl.SelectedIndex)
            If ddl.Items(0).Value = "" Then
                FillCombo(ddl, "EXEC " + SpName, True, SelectValue, SelectName, DBConnection)
            Else
                FillCombo(ddl, "EXEC " + SpName, False, SelectValue, SelectName, DBConnection)
            End If
            If ddl.Items.Contains(LI) Then
                ddl.SelectedValue = LI.Value
            End If
        Catch ex As Exception
            Throw New Exception("Refresh Master Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub BindToText(ByRef TxtBox As TextBox, ByVal Value As String, Optional ByVal DigitDecimal As Integer = -1)
        If DigitDecimal >= 0 Then
            TxtBox.Text = FormatFloat(TrimStr(Value.Replace("&nbsp;", "")), DigitDecimal)
        Else
            TxtBox.Text = TrimStr(Value.Replace("&nbsp;", ""))
        End If
    End Sub

    Public Sub BindToDropList(ByRef DropList As DropDownList, ByVal Value As String)
        If Value.Replace("&nbsp;", "") <> "" Then
            If DropList.Items.Contains(DropList.Items.FindByValue(Value)) Then
                DropList.SelectedValue = Value
            End If
        Else
            Dim LI As ListItem
            LI = New ListItem("Not define", Value)
            DropList.Items.Add(LI)
            DropList.SelectedValue = Value
            'If DropList.Items.Contains(DropList.Items.FindByValue("")) Then
            'DropList.SelectedValue = ""
            'End If            
        End If
    End Sub

    Public Sub BindToDate(ByRef TxtDate As BasicDatePicker, ByVal Value As String)
        If Value.Replace("&nbsp;", "") <> "" Then
            TxtDate.SelectedDate = CDate(Value)
        End If
    End Sub

    Public Sub BindToDateOri(ByRef TxtDate As BasicDatePicker, ByVal Value As DateTime)
        TxtDate.SelectedDate = Value
    End Sub

    Public Sub CheckAll(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbSelect")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub CheckAllAssign(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbAssign")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Public Sub CheckAllAvailable(ByRef gd As GridView, ByVal sender As CheckBox)
        Dim cb, cbselek As CheckBox
        Dim GRW As GridViewRow
        Try
            cb = sender
            For Each GRW In gd.Rows
                cbselek = GRW.FindControl("cbAvailable")
                cbselek.Checked = cb.Checked
            Next
        Catch ex As Exception
            Throw New Exception("Check All Error : " + ex.ToString)
        End Try
    End Sub

    Public Function GetNewItemNo(ByVal Dt As DataTable) As String
        Dim Row As DataRow()
        Dim R As DataRow
        Dim MaxItem As Integer = 0
        Row = Dt.Select("ItemNo IS NOT NULL")
        For Each R In Row
            If CInt(R("ItemNo").ToString) > MaxItem Then
                MaxItem = CInt(R("ItemNo").ToString)
            End If
        Next
        MaxItem = MaxItem + 1
        Return CStr(MaxItem)
    End Function

    Public Sub ChangeFgSubLed(ByVal txFgSubLed As TextBox, ByRef txCode As TextBox, ByRef btn As Button)
        txCode.Enabled = txFgSubLed.Text <> "N"
        btn.Visible = txCode.Enabled
    End Sub

    Public Function EnableCostCtr(ByVal FgType As String) As Boolean
        Return FgType = "PL"
    End Function

    Public Function TrimStr(ByVal Value As String) As String
        Return Value.Replace("&nbsp;", "")
    End Function

    Public Sub ChangePaymentType(ByVal Payment As String, ByRef TxFgMode As TextBox, ByVal txdate As BasicDatePicker, ByRef txduedate As BasicDatePicker, ByRef ddlbank As DropDownList, ByRef ddlCurr As DropDownList, ByRef ddlChargeCurr As DropDownList, ByRef txRate As TextBox, ByRef txChargeRate As TextBox, ByRef txChargeForex As TextBox, ByVal HomeCurrency As String, ByVal DigitCurr As Integer, Optional ByVal DBConnection As String = "Nothing", Optional ByVal State As String = "Add")
        Try
            If State = "Add" Then
                Dim dr As DataRow
                TxFgMode.Text = "O"
                If Not Payment.Trim = "" Then
                    dr = FindMaster("PayType", Payment, DBConnection)
                    If Not dr Is Nothing Then
                        BindToText(TxFgMode, dr("FgMode").ToString)
                        BindToDropList(ddlCurr, dr("Currency").ToString)
                    End If
                End If
                If Not TxFgMode.Text = "G" Then
                    txduedate.SelectedDate = Nothing
                    txduedate.DisplayType = DatePickerDisplayType.TextBox
                    ddlbank.SelectedIndex = 0
                Else
                    txduedate.SelectedDate = txdate.SelectedDate
                    txduedate.DisplayType = DatePickerDisplayType.TextBoxAndImage
                End If
                If Not TxFgMode.Text = "B" Then
                    ddlChargeCurr.SelectedIndex = 0
                    txChargeRate.Enabled = False
                    txChargeForex.Text = "0"
                Else
                    ddlChargeCurr.SelectedValue = ddlCurr.SelectedValue
                    txChargeRate.Enabled = True
                    ChangeCurrency(ddlChargeCurr, txdate, txChargeRate, HomeCurrency, DigitCurr, DBConnection)
                End If
                ChangeCurrency(ddlCurr, txdate, txRate, HomeCurrency, DigitCurr, DBConnection)
            End If
            txduedate.Enabled = TxFgMode.Text = "G"
            ddlbank.Enabled = TxFgMode.Text = "G"
            ddlChargeCurr.Enabled = TxFgMode.Text = "B"
            txChargeForex.Enabled = TxFgMode.Text = "B"
        Catch ex As Exception
            Throw New Exception("Change Payment Error : " + ex.ToString)
        End Try
    End Sub
    Public Sub ChangePaymentTypeV2(ByVal Payment As String, ByRef TxFgMode As TextBox, ByVal txdate As BasicDatePicker, ByRef txduedate As BasicDatePicker, ByRef ddlbank As DropDownList, ByRef ddlCurr As DropDownList, ByRef txRate As TextBox, ByVal HomeCurrency As String, ByVal DigitCurr As Integer, Optional ByVal DBConnection As String = "Nothing", Optional ByVal State As String = "Add")
        Try
            If State = "Add" Then
                Dim dr As DataRow
                TxFgMode.Text = "O"
                If Not Payment.Trim = "" Then
                    dr = FindMaster("PayType", Payment, DBConnection)
                    If Not dr Is Nothing Then
                        BindToText(TxFgMode, dr("FgMode").ToString)
                        BindToDropList(ddlCurr, dr("Currency").ToString)
                    End If
                End If
                If Not TxFgMode.Text = "G" Then
                    txduedate.SelectedDate = Nothing
                    txduedate.DisplayType = DatePickerDisplayType.TextBox
                    ddlbank.SelectedIndex = 0
                Else
                    txduedate.SelectedDate = txdate.SelectedDate
                    txduedate.DisplayType = DatePickerDisplayType.TextBoxAndImage
                End If
                If Not TxFgMode.Text = "B" Then
                    ' ddlChargeCurr.SelectedIndex = 0
                    ' txChargeRate.Enabled = False
                    ' txChargeForex.Text = "0"
                Else
                    ' ddlChargeCurr.SelectedValue = ddlCurr.SelectedValue
                    ' txChargeRate.Enabled = True
                    'ChangeCurrency(txdate, HomeCurrency, DigitCurr, DBConnection)
                End If
                ChangeCurrency(ddlCurr, txdate, txRate, HomeCurrency, DigitCurr, DBConnection)
            End If
            txduedate.Enabled = TxFgMode.Text = "G"
            ddlbank.Enabled = TxFgMode.Text = "G"
            ' ddlChargeCurr.Enabled = TxFgMode.Text = "B"
            ' txChargeForex.Enabled = TxFgMode.Text = "B"
        Catch ex As Exception
            Throw New Exception("Change Payment Error : " + ex.ToString)
        End Try
    End Sub
    Public Function CoalesceZero(ByVal val As String) As String
        If val.Trim = "" Then
            Return "0"
        Else
            Return val
        End If
    End Function

    Public Function CekRangePeriodSelected(ByVal StartYear As Integer, ByVal StartMonth As Integer, ByVal EndYear As Integer, ByVal EndMonth As Integer) As String
        Dim hasil As String
        hasil = ""
        If StartYear > EndYear Then
            hasil = "Start Year (" + Str(StartYear) + ") cannot greater than End Year (" + Str(EndYear) + ")"
        Else
            If StartYear = EndYear Then
                If StartMonth > EndMonth Then
                    hasil = "Start Month cannot greater than End Month"
                End If
            End If
        End If
        Return hasil
    End Function

End Module
