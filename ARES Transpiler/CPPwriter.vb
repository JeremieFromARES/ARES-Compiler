Public Class CPPwriter

    Public Shared cpp_lines As New List(Of String)
    Public Shared cpp_oh_proto As New List(Of String)
    Public Shared final_cpp_lines As New List(Of String)

    Public Shared cpp_declared_functions As New Dictionary(Of String, FunctionObject)
    Public Shared cpp_declared_variables As New Dictionary(Of String, String)

    Public Class FunctionObject

        Public Property return_type As String = String.Empty
        Public Property argument_types As New List(Of String)
    End Class

    Public Shared Sub FinilizeLines()

        final_cpp_lines.AddRange(cpp_oh_proto)
        final_cpp_lines.Add("// ARES - Program")
        final_cpp_lines.AddRange(cpp_lines)
    End Sub

    Public Shared Sub DefineOverheadPrototypes()

        Dim counter As Long
        Dim temp_line As String
        cpp_oh_proto.Add("// ARES - Overhead prototypes")
        For Each func In cpp_declared_functions

            If func.Value.return_type = "" And func.Key = "main" Then
                temp_line = "int " + func.Key + "( "
            ElseIf func.Value.return_type = "" Then
                temp_line = "void " + func.Key + "( "
            Else
                temp_line = ARES.typesToCPP(func.Value.return_type) + " " + func.Key + "( "
            End If

            counter = 0
            For Each arg_type In func.Value.argument_types

                counter += 1
                temp_line += arg_type + " " + "x" & counter & ","
            Next

            temp_line = Left(temp_line, temp_line.Length - 1)
            temp_line += ");"
            cpp_oh_proto.Add(temp_line)
        Next
        cpp_oh_proto.Add("")
    End Sub

    Public Shared Sub InitalHeaders()

        cpp_oh_proto.Add("// using the ARES language 2022, under GPL3 license.")
        cpp_oh_proto.Add("// Transpile date: " & Format(Now, "yyyy/mm/dd hh:mm:ss"))
        cpp_oh_proto.Add("")
        cpp_oh_proto.Add("// ARES - Headers")
        cpp_oh_proto.Add("#include ""ARES.h""")
        cpp_oh_proto.Add("")
    End Sub

    Public Shared Sub DeclareFunction(ByRef function_name As String, ByRef arguments As List(Of TokenCollection), Optional ByRef return_type As String = "")

        Dim temp_line As String
        Dim temp_arg_types As New List(Of String)

        If function_name <> "main" Then
            If return_type <> "" Then ' Get return type

                temp_line = ARES.typesToCPP(return_type)
            Else

                temp_line = "void"
            End If
        Else

            temp_line = "int"
        End If

        temp_line += $" {function_name}( "

        For Each arg In arguments ' Get function arguments

            temp_line += " "

            Debug.Print(arg.token)

            If arg.type = TokenType.IsType Then

                temp_line += ARES.typesToCPP(arg.token)
                temp_arg_types.Add(ARES.typesToCPP(arg.token))

            ElseIf arg.type = TokenType.IsName Then

                temp_line += arg.token

            ElseIf arg.type = TokenType.IsOperator Then

                temp_line += arg.token

            ElseIf arg.type = TokenType.ArgDelimiter Then

                temp_line += ","

            ElseIf arg.type = TokenType.ArgStart Then

            End If
        Next

        temp_line = Left(temp_line, temp_line.Length - 1) ' Remove last ","
        WriteCPP(temp_line + ") {")
        If return_type <> "" Then

            WriteCPP(ARES.typesToCPP(return_type) + " __ARES_return__;")
        End If

        Dim FcObj As New FunctionObject
        FcObj.return_type = return_type
        FcObj.argument_types = temp_arg_types

        cpp_declared_functions.Add(function_name, FcObj)
    End Sub

    Private Shared Sub WriteArgs(ByRef arguments As List(Of TokenCollection))

    End Sub

    Public Shared Sub DeclareObject(ByRef object_name As String)

        WriteCPP("class " + object_name + " {")
        WriteCPP("public:")

        cpp_declared_variables.Add(object_name, "Obj")
    End Sub

    Public Shared Sub DeclareVariable(ByRef variable_name As String, ByRef variable_type As String)

        WriteCPP(ARES.typesToCPP(variable_type) + $" {variable_name};")

        cpp_declared_variables.Add(variable_name, variable_type)
    End Sub

    Public Shared Sub DeclaredEnd()

        WriteCPP("};")
    End Sub

    Public Shared Sub DeclareReturn()

        WriteCPP("return __ARES_return__;")
        WriteCPP("};")
    End Sub

    Public Shared Sub CallFunction(ByRef function_name As String, ByRef arguments As List(Of TokenCollection))

        Dim temp_line As String

        If cpp_declared_functions.ContainsKey(function_name) Then

            temp_line = function_name ' + "("

        ElseIf ARES.standard_functions.Contains(function_name) Then

            temp_line = "ARES::" + function_name ' + "("
        Else

            temp_line = function_name ' + "("
        End If

        For Each arg In arguments ' Get function arguments

            If arg.type = TokenType.ArgDelimiter Then

                temp_line += ","

            ElseIf arg.type = TokenType.ArgStart Then

                temp_line += "("

            ElseIf arg.type = TokenType.Terminator Then

                temp_line += ")"

            ElseIf arg.type = TokenType.IsName Then

                If cpp_declared_functions.ContainsKey(arg.token) Then

                    temp_line += " " + arg.token ' + "("

                ElseIf cpp_declared_variables.ContainsKey(arg.token) Then

                    temp_line += " " + arg.token

                ElseIf ARES.standard_functions.Contains(arg.token) Then

                    temp_line += " ARES::" + arg.token ' + "("

                Else

                    temp_line += arg.token

                End If

            Else
                temp_line += " " + arg.token
            End If
        Next

        WriteCPP(temp_line + ";")
    End Sub

    Public Shared Sub WriteCPP(ByRef text As String)

        cpp_lines.Add(text)
    End Sub
End Class
