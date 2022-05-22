Public Class CPPwriter

    Public Shared cpp_lines As New List(Of String)
    Public Shared cpp_obj_lines As New List(Of String)
    Public Shared cpp_oh_proto As New List(Of String)
    Public Shared final_cpp_lines As New List(Of String)

    Public Shared cpp_declared_functions As New Dictionary(Of String, FunctionObject)
    Public Shared cpp_declared_variables As New Dictionary(Of String, String)
    Public Shared cpp_declared_objects As New List(Of String)

    Public Shared cpp_pre_declared_functions As New List(Of String)
    Public Shared cpp_pre_declared_variables As New List(Of String)
    Public Shared cpp_pre_declared_objects As New List(Of String)

    Public Class FunctionObject

        Public Property return_type As String = String.Empty
        Public Property argument_types As New List(Of String)
    End Class

    Public Shared Sub FinilizeLines()

        final_cpp_lines.AddRange(cpp_oh_proto)
        final_cpp_lines.Add("// ARES - Objects")
        final_cpp_lines.AddRange(cpp_obj_lines)
        final_cpp_lines.Add("")
        final_cpp_lines.Add("// ARES - Program")
        final_cpp_lines.AddRange(cpp_lines)
    End Sub

    Public Shared Sub DefineOverheadPrototypes()

        Dim counter As Long
        Dim temp_line As String
        cpp_oh_proto.Add("// ARES - Overhead prototypes")
        For Each func In cpp_declared_functions

            Debug.Print("Added " & func.Key & " to C++ Overhead Prototypes.")

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

        cpp_oh_proto.Add("// Using the ARES language 2022, under GPL3 license.")
        cpp_oh_proto.Add("// Transpile date: " & Format(Now, "yyyy/mm/dd hh:mm:ss"))
        cpp_oh_proto.Add("")
        cpp_oh_proto.Add("// ARES - Headers")
        cpp_oh_proto.Add("#include ""ARES.h""")
        cpp_oh_proto.Add("")
    End Sub

    Public Shared Sub PreDeclareFunction(ByRef function_name As String)

        Debug.Print("Pre-Declared " & function_name & " As Function.")
        cpp_pre_declared_functions.Add(function_name)

    End Sub

    Public Shared Sub DeclareFunction(ByRef function_name As String, ByRef arguments As List(Of TokenCollection), Optional ByRef return_type As String = "")

        Debug.Print("Declared  " & function_name & " As Function.")

        Translator.indentation += 1

        Dim temp_line As String
        Dim temp_arg_types As New List(Of String)
        Dim mainfunc As Boolean = False

        If function_name <> "main" Then
            If return_type <> "" Then ' Get return type

                temp_line = ARES.typesToCPP(return_type)
            Else

                temp_line = "void"
            End If
        Else

            temp_line = "int"
            mainfunc = True
        End If

        temp_line += $" {function_name}( "

        For Each arg In arguments ' Get function arguments

            temp_line += " "

            If arg.type = TokenType.IsType Then

                temp_line += ARES.typesToCPP(arg.token)
                temp_arg_types.Add(ARES.typesToCPP(arg.token))
            ElseIf arg.type = TokenType.IsName Then

                temp_line += arg.token

                Debug.Print("Declared " & arg.token & " As Variable since is argument.")
                If Not cpp_declared_variables.ContainsKey(arg.token) Then
                    cpp_declared_variables.Add(arg.token, "")
                End If
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

        If mainfunc Then

            WriteCPP("ARES::__ARES_MAIN_INIT__();")
        End If

        Dim FcObj As New FunctionObject
        FcObj.return_type = return_type
        FcObj.argument_types = temp_arg_types

        If Not cpp_declared_functions.ContainsKey(function_name) Then
            cpp_declared_functions.Add(function_name, FcObj)
        End If
    End Sub

    Public Shared Sub PreDeclareObject(ByRef object_name As String)

        Debug.Print("Pre-Declared " & object_name & " As Object.")
        cpp_pre_declared_objects.Add(object_name)

    End Sub

    Public Shared Sub DeclareObject(ByRef object_name As String)

        Debug.Print("Declared " & object_name & " As Object.")

        Translator.object_def = True

        If Translator.indentation <> 0 Then

            ErrorHandler.PrintError("Syntax", "cannot define Object outside of global scope.", Translator.line_counter)
        End If

        Translator.indentation += 1

        WriteCPP("class " + object_name + " {")
        WriteCPP("public:")

        cpp_declared_objects.Add(object_name)
    End Sub

    Public Shared Sub PreDeclareVariable(ByRef variable_name As String)

        Debug.Print("Pre-Declared " & variable_name & " As Variable.")
        cpp_pre_declared_variables.Add(variable_name)

    End Sub

    Public Shared Sub DeclareVariable(ByRef variable_name As String, ByRef variable_type As String, ByRef arguments As List(Of TokenCollection))

        Debug.Print("Declared " & variable_name & " As Variable.")

        Dim templine As String = String.Empty

        If ARES.types.Contains(variable_type) Then

            templine += ARES.typesToCPP(variable_type) + $" {variable_name}"

        ElseIf cpp_pre_declared_objects.Contains(variable_type) Then

            templine += variable_type + $" {variable_name}"

        ElseIf ARES.standard_objects.Contains(variable_type) Then

            templine += variable_type + $" {variable_name}"
        Else

            ErrorHandler.PrintError("Syntax", $"type {variable_type} does not exist.", Translator.line_counter)
        End If

        If arguments.Count > 0 Then
            If arguments(0).type = TokenType.IsOperator Then

                templine += TranslateArgs(arguments)
            Else

                ErrorHandler.PrintError("Syntax", $"expected operator after {variable_name}.", Translator.line_counter)
            End If
        End If

        templine += ";"
        WriteCPP(templine)

        If Not cpp_declared_variables.ContainsKey(variable_name) Then
            cpp_declared_variables.Add(variable_name, variable_type)
        End If
    End Sub

    Public Shared Sub AssignToVariable(ByRef variable_name As String, ByRef arguments As List(Of TokenCollection))

        Dim templine As String = String.Empty

        templine += variable_name

        If arguments.Count > 0 Then
            If arguments(0).type = TokenType.IsOperator Then

                templine += TranslateArgs(arguments)
            Else

                ErrorHandler.PrintError("Syntax", $"expected operator after {variable_name}.", Translator.line_counter)
            End If
        Else

            ErrorHandler.PrintError("Syntax", $"expected operator after {variable_name}.", Translator.line_counter)
        End If

        templine += ";"
        WriteCPP(templine)
    End Sub

    Public Shared Sub DeclaredEnd()

        Translator.indentation -= 1

        WriteCPP("};")

        If Translator.indentation = 0 Then Translator.object_def = False
    End Sub

    Public Shared Sub DeclareReturn(ByRef arguments As List(Of TokenCollection))

        Translator.indentation -= 1

        If arguments.Count > 0 Then

            Dim templine = "__ARES_return__ = "
            templine += TranslateArgs(arguments)
            templine += ";"
            WriteCPP(templine)
        Else
            ErrorHandler.PrintError("Syntax", $"expected operator after Return.", Translator.line_counter)
        End If

        WriteCPP("return __ARES_return__;")
        WriteCPP("};")

        If Translator.indentation = 0 Then Translator.object_def = False
    End Sub

    Public Shared Sub CallFunction(ByRef function_name As String, ByRef arguments As List(Of TokenCollection))

        Dim temp_line As String

        If cpp_pre_declared_functions.Contains(function_name) Then

            temp_line = function_name

        ElseIf function_name = "Cpp" Then

            ErrorHandler.PrintWarning("Prevention", "the function Cpp prevents ARES to check for errors. Please use at your own risks.", Translator.line_counter)

            Dim argcount As Integer = 0
            For Each token In arguments
                If token.token.Length <= 0 Then Continue For
                argcount += 1

                If Not token.token.Contains("""") Then
                    ErrorHandler.PrintError("Syntax", "the function Cpp only takes constant strings as arguments due to it being a translation-time function.", Translator.line_counter)
                End If

                temp_line += Left(Right(token.token.Replace("\""", """"), token.token.Length - 1), token.token.Length - 2)
            Next

            If argcount < 1 Then
                ErrorHandler.PrintError("Syntax", "the function Cpp does not accept more than one arguments.", Translator.line_counter)
            End If

            GoTo Skipargs

        ElseIf ARES.standard_functions.Contains(function_name) Then

            temp_line = "ARES::" + function_name

        ElseIf function_name.Contains(".") Then

            temp_line += OOPhandler(function_name)
        Else

            ErrorHandler.PrintError("Syntax", $"function {function_name} is not defined.", Translator.line_counter)
            'temp_line = function_name
        End If

        temp_line += TranslateArgs(arguments)
        WriteCPP(temp_line + ";")
        Exit Sub
Skipargs:
        WriteCPP(temp_line)
    End Sub

    Public Shared Sub DeclareIf(ByRef arguments As List(Of TokenCollection), Optional ByRef prefix As String = "")

        Translator.indentation += 1

        Dim temp_line As String
        temp_line = prefix

        Dim temp_tokens As List(Of TokenCollection) = arguments

        If temp_tokens.Count <> 0 Then

            temp_line += $"if ({TranslateArgs(temp_tokens)}) "
        End If

        temp_line += "{"
        WriteCPP(temp_line)
    End Sub

    Public Shared Sub DeclareLoop(ByRef arguments As List(Of TokenCollection))

        Translator.indentation += 1

        Dim temp_line As String

        Dim temp_tokens As List(Of TokenCollection) = arguments

        If temp_tokens.Count <> 0 Then

            temp_line += $"while ({TranslateArgs(temp_tokens)}) "
        Else

            temp_line += "while (1) "
        End If

        temp_line += "{"
        WriteCPP(temp_line)
    End Sub

    Public Shared Function OOPhandler(ByRef function_name As String) As String

        Dim temp_line As String = String.Empty

        Dim temp_split() As String = Split(function_name, ".")
        Dim temp_func As String = temp_split(temp_split.Count - 1)

        For Each tmp In temp_split

            If tmp <> temp_func Then

                If cpp_declared_variables.ContainsKey(tmp) Then

                    temp_line += $"{tmp}."

                ElseIf ARES.standard_objects.Contains(tmp) Then

                    temp_line += $"ARES::{tmp}::"

                Else

                    Dim x As Integer
                    If Not Integer.TryParse(tmp, x) Then
                        ErrorHandler.PrintError("Syntax", $"object {tmp} is not defined.", Translator.line_counter)
                    End If
                    'temp_line += $"{tmp}."
                End If
            Else

                If cpp_pre_declared_functions.Contains(temp_func) Then
                    temp_line += temp_func

                ElseIf ARES.standard_functions.Contains(temp_func) Then
                    temp_line += temp_func

                Else
                    ErrorHandler.PrintError("Syntax", $"function {temp_func} is not defined.", Translator.line_counter)
                End If
            End If
        Next

        OOPhandler = temp_line

    End Function

    Public Shared Function TranslateArgs(ByRef arguments As List(Of TokenCollection))

        Dim temp_line = String.Empty

        For Each arg In arguments ' Get function arguments

            If arg.type = TokenType.ArgDelimiter Then

                temp_line += ","

            ElseIf arg.type = TokenType.ArgStart Then

                temp_line += "("

            ElseIf arg.type = TokenType.Terminator Then

                temp_line += ")"

            ElseIf arg.type = TokenType.IsName Then

                Dim temp_split() As String = Split(arg.token, ".")
                Dim temp_func As String = temp_split(temp_split.Count - 1)

                If cpp_declared_functions.ContainsKey(arg.token) Then

                    temp_line += $" {arg.token}"

                ElseIf cpp_declared_variables.ContainsKey(arg.token) Then

                    temp_line += $" {arg.token}"

                ElseIf ARES.standard_functions.Contains(arg.token) Then

                    temp_line += $" ARES::{arg.token}"

                ElseIf arg.token.Contains(".") Then

                    temp_line += OOPhandler(arg.token)

                Else

                    temp_line += arg.token
                End If

            Else
                temp_line += " " + arg.token
            End If
        Next

        TranslateArgs = temp_line

    End Function

    Public Shared Sub WriteCPP(ByRef text As String)

        If Translator.object_def Then

            cpp_obj_lines.Add(text)
        Else

            cpp_lines.Add(text)
        End If
    End Sub
End Class
