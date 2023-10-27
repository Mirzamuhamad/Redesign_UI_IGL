Imports System.Data
'Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.Configuration

Public Module DataAccess
    'Dim ConnString As String = System.Configuration.ConfigurationManager.AppSettings.Get("ConString")
    'Dim ConnString As String = "Data Source=10.10.10.12;Initial Catalog=ASPAC;Persist Security Info=True;User ID=sa;Password="
    Dim ConnString As String = "Data Source=" + System.Configuration.ConfigurationManager.AppSettings.Get("ServerIP") + ";Initial Catalog=ASPACXX;User ID=dataprimaaccess;Password=d@t@prima.#;Connection Timeout=600"

    Public Function GetConString() As String
        Return ConnString
    End Function

    Public Function GetSchema(ByVal SqlString As String, ByVal Connection As String) As DataTable
        Dim dt As New DataTable
        Dim Mycon As New SqlConnection
        Dim MyDa As New SqlDataAdapter
        Try
            'If Connection = "Nothing" Then
            'Mycon = New SqlConnection(ConnString)
            'Else
            Mycon = New SqlConnection(Connection)
            'End If
            MyDa = New SqlDataAdapter(SqlString, Mycon)
            MyDa.FillSchema(dt, SchemaType.Source)

            Return dt
        Catch ex As Exception
            Throw New Exception("GetSchema Error: " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & SqlString & vbCrLf)
        End Try
    End Function
    Public Function SQLExecuteNonQuery(ByRef SQLString As String, ByVal Connection As String) As Long
        Dim cmd As New SqlCommand
        Dim recordsAffected As Long = 0
        Dim Mycon As New SqlConnection
        Dim MyDa As New SqlDataAdapter

        Try
            'If Connection = "Nothing" Then
            'Mycon = New SqlConnection(ConnString)
            'Else
            Mycon = New SqlConnection(Connection)
            'End If

            cmd = New SqlCommand(SQLString, Mycon)
            cmd.CommandTimeout = 300
            Mycon.Open()
            recordsAffected = cmd.ExecuteNonQuery()

            Return recordsAffected
        Catch ex As Exception
            Throw New Exception("SQLExecuteNonQuery Error: " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & SQLString & vbCrLf)
        Finally
            If Not Mycon Is Nothing Then Mycon.Dispose()
            If Not cmd Is Nothing Then cmd.Dispose()
        End Try
    End Function

    Public Function SQLExecuteQuery(ByRef SQLString As String, ByVal Connection As String) As DataSet
        Dim ds As New DataSet
        Dim Mycon As New SqlConnection
        Dim MyDa As New SqlDataAdapter
        Try
            'If Connection = "Nothing" Then
            'Mycon = New SqlConnection(ConnString)
            'Else
            Mycon = New SqlConnection(Connection)
            'End If
            MyDa = New SqlDataAdapter(SQLString, Mycon)
            MyDa.SelectCommand.CommandTimeout = 360
            Mycon.Open()
            MyDa.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception("SQLExecuteQuery Error: " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & SQLString)
        Finally
            If Not Mycon Is Nothing Then Mycon.Dispose()
            If Not MyDa Is Nothing Then MyDa.Dispose()
        End Try
    End Function

    Public Function SQLExecuteReader(ByRef SQLString As String, ByVal Connection As String) As SqlDataReader
        Dim Mycon As New SqlConnection
        Dim myCommand As SqlCommand
        Try
            'If Connection = "Nothing" Then
            'Mycon = New SqlConnection(ConnString)
            'Else
            Mycon = New SqlConnection(Connection)
            'End If
            Mycon.Open()
            myCommand = New SqlCommand(SQLString, Mycon)

            Return myCommand.ExecuteReader(CommandBehavior.CloseConnection)
        Catch ex As Exception
            Throw New Exception("SQLExecuteQuery Error: " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & SQLString)
        Finally
            'If Not Mycon Is Nothing Then Mycon.Dispose()
        End Try
    End Function

    Public Function SQLExecuteScalar(ByRef SQLString As String, ByVal Connection As String) As String
        Dim cn As New SqlConnection
        Dim cmd As New SqlCommand
        Dim Obj As Object
        Dim Result As String
        Try
            'If Connection = "Nothing" Then
            'cn = New SqlConnection(ConnString)
            'Else
            cn = New SqlConnection(Connection)
            'End If

            cmd = New SqlCommand(SQLString, cn)
            cmd.CommandTimeout = 300
            cn.Open()
            Obj = cmd.ExecuteScalar
            If IsDBNull(Obj) Then
                Result = "0"
            Else
                If IsNothing(Obj) Then
                    Result = ""
                Else
                    Result = Obj.ToString
                End If
            End If

            Return Result
        Catch ex As Exception
            Throw New Exception("SQLExecuteScalar  Exception: " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & SQLString & vbCrLf)
        Finally
            If Not cn Is Nothing Then cn.Dispose()
            If Not cmd Is Nothing Then cmd.Dispose()
        End Try
    End Function

    Public Function ExecSPPosting(ByVal ProcName As String, ByVal Nmbr As String, ByVal Tahun As Integer, ByVal Period As Integer, ByVal UserId As String, ByVal Connection As String) As String
        Dim Mycon As New SqlConnection
        Dim Hasil As String
        Dim PrimaryKey() As String
        PrimaryKey = Nmbr.Split("|")
        Try
            'If Connection = "Nothing" Then
            'Mycon = New SqlConnection(ConnString)
            'Else
            Mycon = New SqlConnection(Connection)
            'End If

            Dim sqlstring As String
            sqlstring = ""
            If PrimaryKey.Length = 1 Then
                sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + Tahun.ToString + ", " + Period.ToString + ", " + QuotedStr(UserId) + ", @A OUT " + _
                            "SELECT @A"
            ElseIf PrimaryKey.Length = 2 Then
                sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + Tahun.ToString + ", " + Period.ToString + ", " + QuotedStr(UserId) + ", @A OUT " + _
                            "SELECT @A;"
            ElseIf PrimaryKey.Length = 3 Then
                sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + Tahun.ToString + ", " + Period.ToString + ", " + QuotedStr(UserId) + ", @A OUT " + _
                            "SELECT @A;"
            ElseIf PrimaryKey.Length = 4 Then
                sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + QuotedStr(PrimaryKey(3).ToString) + "," + Tahun.ToString + ", " + Period.ToString + ", " + QuotedStr(UserId) + ", @A OUT " + _
                            "SELECT @A;"
            ElseIf PrimaryKey.Length = 5 Then
                sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + QuotedStr(PrimaryKey(3).ToString) + "," + QuotedStr(PrimaryKey(4).ToString) + "," + Tahun.ToString + ", " + Period.ToString + ", " + QuotedStr(UserId) + ", @A OUT " + _
                            "SELECT @A;"
            ElseIf PrimaryKey.Length = 6 Then
                sqlstring = "DECLARE @A VARCHAR(255) " + _
                            "EXEC " + ProcName + " " + QuotedStr(PrimaryKey(0).ToString) + ", " + QuotedStr(PrimaryKey(1).ToString) + ", " + QuotedStr(PrimaryKey(2).ToString) + "," + QuotedStr(PrimaryKey(3).ToString) + "," + QuotedStr(PrimaryKey(4).ToString) + "," + QuotedStr(PrimaryKey(5).ToString) + "," + Tahun.ToString + ", " + Period.ToString + ", " + QuotedStr(UserId) + ", @A OUT " + _
                            "SELECT @A;"
            End If
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            If Hasil.Length > 1 Then
                Return Hasil
            Else
                Return Hasil.Replace("0", "")
            End If
        Catch ex As Exception
            Throw New Exception("Exec SP Posting Error : " + ex.ToString)
        Finally
            Mycon.Close()
        End Try
    End Function

    Public Sub ExecSP(ByVal ProcName As String, ByVal Params As SqlParameterCollection, ByVal Connection As String)
        'if there are no params, fill nothing exp: ExecSP(S_FindRate,Nothing)
        Dim Mycon As New SqlConnection
        Dim MyDa As New SqlDataAdapter
        Dim cmd As New SqlCommand
        Dim Param As SqlParameter
        Try
            'If Connection = "Nothing" Then
            'Mycon = New SqlConnection(ConnString)
            'Else
            Mycon = New SqlConnection(Connection)
            'End If
            cmd = New SqlCommand(ProcName, Mycon)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 300

            If Not Params Is Nothing Then
                For Each Param In Params
                    cmd.Parameters.Add(Param)
                Next
            End If

            Mycon.Open()
            cmd.ExecuteNonQuery()

            cmd.Connection.Close()
            Mycon.Close()

        Catch ex As Exception
            Throw New Exception("Exec SP Error : " + ex.ToString)
        Finally
            cmd.Connection.Close()
            Mycon.Close()
        End Try
    End Sub

    Public Function ExecSPParamsOut(ByVal ProcName As String, ByVal Params As SqlParameterCollection, ByVal Connection As String) As String
        Dim Mycon As New SqlConnection
        Dim MyDa As New SqlDataAdapter
        Dim cmd As New SqlCommand
        Dim Param As SqlParameter
        Dim hasil As String
        Try
            'If Connection = "Nothing" Then
            'Mycon = New SqlConnection(ConnString)
            'Else
            Mycon = New SqlConnection(Connection)
            'End If
            cmd = New SqlCommand(ProcName, Mycon)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 300
            If Not Params Is Nothing Then
                For Each Param In Params
                    cmd.Parameters.Add(Param)
                Next
            End If

            Dim EMessage As New SqlParameter("Emessage", SqlDbType.VarChar, 255)
            EMessage.Direction = ParameterDirection.Output
            cmd.Parameters.Add(EMessage)

            Mycon.Open()
            cmd.ExecuteNonQuery()

            If IsDBNull(cmd.Parameters("EMessage").Value) Then
                hasil = ""
            Else
                hasil = cmd.Parameters("EMessage").Value
            End If

            cmd.Connection.Close()
            Mycon.Close()

            Return hasil

        Catch ex As Exception
            Throw New Exception("Exec SP with param out : " + ex.ToString)
        End Try
    End Function

    Public Function SetMenuLevel(ByVal ContainerName As String, ByVal UserId As String, ByVal Connection As String) As DataTable
        Dim tesDS As DataSet
        Dim DT As DataTable
        Try
            tesDS = SQLExecuteQuery("EXEC S_SAGetMenuLevel " + QuotedStr(UserId) + ", " + QuotedStr(ContainerName), Connection)
            DT = tesDS.Tables(0)
            Return DT
        Catch ex As Exception
            Throw New Exception("SetMenuLevel Error : " + ex.ToString)
        End Try
    End Function

    Public Function FindMaster(ByVal master As String, ByVal code As String, ByVal Connection As String) As DataRow
        Dim DR As DataRow
        Dim DT As DataTable
        Dim SPName As String
        Dim Param() As String
        Try
            SPName = ""
            Param = code.Split("|")
            If Param.Length = 1 Then
                SPName = "EXEC S_Find" + master + " " + QuotedStr(Param(0).Trim)
            ElseIf Param.Length = 2 Then
                SPName = "EXEC S_Find" + master + " " + QuotedStr(Param(0).Trim) + ", " + QuotedStr(Param(1).Trim)
            End If
            DT = SQLExecuteQuery(SPName, Connection).Tables(0)
            If DT.Rows.Count > 0 Then
                DR = DT.Rows(0)
            Else
                DR = Nothing
            End If
            Return DR
        Catch ex As Exception
            Throw New Exception("Find Master Error : " + ex.ToString)
        End Try
    End Function

    Public Function ExecSPCommandGo(ByVal Value As String, ByVal SpName As String, ByVal nmbr As String, ByVal GLYear As Integer, ByVal GLPeriod As Integer, ByVal userId As String, ByVal Connection As String) As String
        Try
            Select Case Value
                Case "Get Approval"
                    Return ExecSPPosting(SpName + "GetAppr", nmbr, GLYear, GLPeriod, userId, Connection)
                Case "Post"
                    Return ExecSPPosting(SpName + "Post", nmbr, GLYear, GLPeriod, userId, Connection)
                Case "Un-Post"
                    Return ExecSPPosting(SpName + "UnPost", nmbr, GLYear, GLPeriod, userId, Connection)
                Case "Delete"
                    Return ExecSPPosting(SpName + "Delete", nmbr, GLYear, GLPeriod, userId, Connection)
                Case "Complete"
                    Return ExecSPPosting(SpName + "Complete", nmbr, GLYear, GLPeriod, userId, Connection)
                Case "Cancel"
                    Return ExecSPPosting(SpName + "Cancel", nmbr, GLYear, GLPeriod, userId, Connection)
                Case "Un-Complete"
                    Return ExecSPPosting(SpName + "UnComplete", nmbr, GLYear, GLPeriod, userId, Connection)
                Case Else
                    Return ""
            End Select
        Catch ex As Exception
            Throw New Exception("Error EXEC SP Command Go " + ex.ToString)
        End Try
    End Function

    Public Function GetAutoNmbr(ByVal PrmModul As String, ByVal FgReport As String, ByVal tahun As Integer, ByVal Period As Integer, Optional ByVal Addparam As String = "", Optional ByVal Connection As String = "Nothing") As String
        Dim Mycon As New SqlConnection
        Dim sqlstring As String
        Dim Hasil As String
        Try
            If Connection = "Nothing" Then
                Mycon = New SqlConnection(ConnString)
            Else
                Mycon = New SqlConnection(Connection)
            End If
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                        "EXEC S_SAAutoNmbr " + tahun.ToString + ", " + Period.ToString + ", " + QuotedStr(FgReport.ToString) + ", " + QuotedStr(PrmModul) + ", " + QuotedStr(Addparam.Trim) + ", @A OUT " + _
                        "SELECT @A"
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            Return Hasil
        Catch ex As Exception
            Throw New Exception("Exec SP AutoNmbr Error : " + ex.ToString)
        Finally
            Mycon.Close()
        End Try
    End Function

    Public Function GetAutoNmbrParamAll(ByVal PrmModul As String, ByVal FgReport As String, ByVal tahun As Integer, ByVal Period As Integer, Optional ByVal Addparam As String = "", Optional ByVal Connection As String = "Nothing") As String
        Dim Mycon As New SqlConnection
        Dim sqlstring As String
        Dim Hasil As String
        Try
            If Connection = "Nothing" Then
                Mycon = New SqlConnection(ConnString)
            Else
                Mycon = New SqlConnection(Connection)
            End If
            sqlstring = "DECLARE @A VARCHAR(255) " + _
                        "EXEC S_SAAutoNmbrParamAll " + tahun.ToString + ", " + Period.ToString + ", " + QuotedStr(FgReport.ToString) + ", " + QuotedStr(PrmModul) + ", " + QuotedStr(Addparam.Trim) + ", @A OUT " + _
                        "SELECT @A"
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            Return Hasil
        Catch ex As Exception
            Throw New Exception("Exec SP AutoNmbr Error : " + ex.ToString)
        Finally
            Mycon.Close()
        End Try
    End Function

    Public Function FindConvertUnit(ByVal Product As String, ByVal UnitOrder As String, ByVal QtyOrder As Double, ByVal Connection As String) As Double
        Dim sqlstring As String
        Dim Hasil As String
        Try
            'If Connection = "Nothing" Then
            'Connection = ConnString
            'End If
            sqlstring = "EXEC S_FindConvertUnit " + QuotedStr(Product) + ", " + QuotedStr(UnitOrder) + ", " + QtyOrder.ToString.Replace(",", "")
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            Return Double.Parse(Hasil)
        Catch ex As Exception
            Throw New Exception("Exec SP Find Convert Unit Error : " + ex.ToString)
        Finally

        End Try
    End Function

    Public Function FindConvertUnitOrder(ByVal Product As String, ByVal UnitOrder As String, ByVal QtyWrhs As Double, ByVal Connection As String) As Double
        Dim sqlstring As String
        Dim Hasil As String
        Try
            'If Connection = "Nothing" Then
            'Connection = ConnString
            'End If
            sqlstring = "EXEC S_FindConvertUnitOrder " + QuotedStr(Product) + ", " + QuotedStr(UnitOrder) + ", " + QtyWrhs.ToString
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            Return Double.Parse(Hasil)
        Catch ex As Exception
            Throw New Exception("Exec SP Find Convert Unit Error : " + ex.ToString)
        Finally

        End Try
    End Function

    Public Function FindDueDate(ByVal Term As String, ByVal Tgl As DateTime, ByVal Connection As String) As DateTime
        Dim sqlstring As String
        Dim Hasil As DateTime
        Try
            'If Connection = "Nothing" Then
            'Connection = ConnString
            'End If
            sqlstring = "EXEC S_FindDueDate " + QuotedStr(Term) + ", " + QuotedStr(Format(Tgl, "yyyy-MM-dd"))
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            Return (Hasil)
        Catch ex As Exception
            Throw New Exception("Exec SP Find Due Date Error : " + ex.ToString)
        Finally

        End Try
    End Function

    Public Function FindTaxRate(ByVal Currency As String, ByVal Tgl As DateTime, ByVal Connection As String) As Double
        Dim dt As DataTable
        Dim dr As DataRow
        Dim sqlstring As String
        Try
            'If Connection = "Nothing" Then
            'Connection = ConnString
            'End If

            sqlstring = "EXEC S_FindRateTax '" + Currency + "', '" + Format(Tgl, "yyyy-MM-dd") + "'"
            dt = SQLExecuteQuery(sqlstring, Connection).Tables(0)
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                Return dr(0)
            Else
                Return 0
            End If

        Catch ex As Exception
            Throw New Exception("Exec SP Find Due Date Error : " + ex.ToString)
        Finally

        End Try
    End Function

    Public Function FindOpnameStart(ByVal FgEmergency As String, ByVal Connection As String) As DateTime
        Dim sqlstring As String
        Dim Hasil As String
        Try
            'If Connection = "Nothing" Then
            'Connection = ConnString
            'End If
            sqlstring = "EXEC S_PDOpnameGetStartDate '" + FgEmergency + "' "
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            Return Hasil
        Catch ex As Exception
            Throw New Exception("Exec SP Find Convert Unit Error : " + ex.ToString)
        Finally

        End Try
    End Function

    Public Function CekExistGiroIn(ByVal GiroNo As String, ByVal Connection As String) As Boolean
        Dim sqlstring As String
        Dim Hasil As String
        Try
            'If Connection = "Nothing" Then
            '    Connection = ConnString
            'End If
            sqlstring = "SELECT GiroNo FROM FINGiroIn WHERE GiroNo = " + QuotedStr(GiroNo)
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            If TrimStr(Hasil.ToString) = "" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw New Exception("Exec SP Find Due Date Error : " + ex.ToString)
        Finally

        End Try
    End Function

    Public Function CekExistGiroOut(ByVal GiroNo As String, ByVal Connection As String) As Boolean
        Dim sqlstring As String
        Dim Hasil As String
        Try
            'If Connection = "Nothing" Then
            '    Connection = ConnString
            'End If
            sqlstring = "SELECT GiroNo FROM FINGiroOut WHERE GiroNo = " + QuotedStr(GiroNo)
            Hasil = SQLExecuteScalar(sqlstring, Connection)
            If TrimStr(Hasil.ToString) = "" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw New Exception("Exec SP Find Due Date Error : " + ex.ToString)
        Finally

        End Try
    End Function
#Region "Not Used"

    'Private Sub GridViewEmpty(ByRef source As DataTable, ByRef gv As GridView)
    '    Try
    '        source.Rows.Add(source.NewRow)
    '        gv.DataSource = source
    '        gv.DataBind()

    '        Dim columnsCount As Integer = gv.Columns.Count

    '        gv.Rows(0).Cells.Clear()

    '        gv.Rows(0).Cells.Add(New TableCell())
    '        gv.Rows(0).Cells(0).ColumnSpan = columnsCount
    '        gv.Rows(0).Cells(0).Text = "NO RESULT FOUND!"
    '    Catch ex As Exception
    '        Throw New Exception("GridViewEmpty Error : " + ex.ToString)
    '    End Try
    'End Sub

    'Public Function ExecSPResultTable(ByVal ProcedureName As String, ByVal Parameters() As SqlParameter) As DataTable
    '    Dim con As New SqlConnection
    '    Dim DBCommand As New SqlCommand
    '    Dim da As New SqlDataAdapter
    '    Dim ObjDataTable As New DataTable
    '    Try
    '        con = New SqlConnection(ConnString)
    '        With DBCommand
    '            .Connection = con
    '            .CommandType = CommandType.StoredProcedure
    '            .CommandText = ProcedureName
    '        End With
    '        Dim iCount As Integer
    '        For iCount = 0 To Parameters.Length() - 1
    '            DBCommand.Parameters.Add(Parameters(iCount))
    '        Next
    '        With da
    '            .SelectCommand = DBCommand
    '            .Fill(ObjDataTable)
    '        End With
    '        Return ObjDataTable
    '    Catch ex As Exception
    '        Throw New Exception("Exec SP Error : " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & ProcedureName)
    '    Finally
    '        If Not da Is Nothing Then da.Dispose()
    '        If Not DBCommand Is Nothing Then DBCommand.Dispose()
    '        If Not con Is Nothing Then con.Dispose()
    '    End Try
    'End Function

    'Public Sub ExecSPResultNon(ByVal ProcedureName As String, ByVal Parameters() As SqlParameter)
    '    'Create an Instance of SQLCommand
    '    Dim con As New SqlConnection
    '    Dim DBCommand = New SqlCommand
    '    Try
    '        con = New SqlConnection(ConnString)
    '        With DBCommand
    '            .Connection = con
    '            .CommandType = CommandType.StoredProcedure
    '            .CommandText = ProcedureName
    '        End With
    '        Dim iCount As Integer
    '        For iCount = 0 To Parameters.Length() - 1
    '            DBCommand.Parameters.Add(Parameters(iCount))
    '        Next
    '        DBCommand.ExecuteNonQuery()
    '    Catch ex As Exception
    '        Throw New Exception("Exec SP Error : " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & ProcedureName)
    '    Finally
    '        If Not DBCommand Is Nothing Then DBCommand.Dispose()
    '        If Not con Is Nothing Then con.Dispose()
    '    End Try

    'End Sub

    'Public Function ExecSPResultParam(ByVal ProcedureName As String, ByVal Parameters() As SqlParameter) As SqlParameterCollection
    '    'Create an Instance of SQLCommand
    '    Dim con As New SqlConnection
    '    Dim DBCommand = New SqlCommand
    '    Try
    '        con = New SqlConnection(ConnString)
    '        With DBCommand
    '            .Connection = con
    '            .CommandType = CommandType.StoredProcedure
    '            .CommandText = ProcedureName
    '        End With
    '        Dim iCount As Integer
    '        For iCount = 0 To Parameters.Length() - 1
    '            DBCommand.Parameters.Add(Parameters(iCount))
    '        Next
    '        DBCommand.ExecuteNonQuery()

    '        Dim ObjParameterCollection As SqlParameterCollection
    '        ObjParameterCollection = DBCommand.Parameters
    '        Return ObjParameterCollection
    '    Catch ex As Exception
    '        Throw New Exception("Exec SP Error : " & vbCrLf & ex.ToString & vbCrLf & "SQLString = " & ProcedureName)
    '    Finally
    '        If Not DBCommand Is Nothing Then DBCommand.Dispose()
    '        If Not con Is Nothing Then con.Dispose()
    '    End Try

    'End Function

#End Region


End Module
