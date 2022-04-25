Imports System.IO
Imports System.Text.RegularExpressions

Public Module Program

    Public translated_to As String = "cpp"

    Public source_file_path As String
    Public source_lines() As String

    Public first_pass_line_objects As New List(Of LineObject)
    Public second_pass_line_objects As New List(Of LineObject)
    Public third_pass_line_objects As New List(Of LineObject)
    Public fourth_pass_line_objects As New List(Of LineObject)

    Public Sub Main()

        Console.WriteLine("ARES Transplier - alpha 1.0.4" & Environment.NewLine)
        Console.WriteLine("Please input the source code file's path or type 0, 1 to try out code snippets.")
        source_file_path = Console.ReadLine()

        Select Case source_file_path ' Code Snippets
            Case "0"
                source_file_path = AppDomain.CurrentDomain.BaseDirectory & "ARES snippets\ARES_test_code.ares"
            Case "1"
                source_file_path = AppDomain.CurrentDomain.BaseDirectory & "ARES snippets\helloworld.ares"
            Case "2"
                source_file_path = AppDomain.CurrentDomain.BaseDirectory & "ARES snippets\operator_testing.ares"
        End Select

        Console.WriteLine("Show parser/transpiler results? (debug purposes). [Y/n]")
        Dim showtranslatorresults As String = Console.ReadLine()

        Parser.Init()

        Dim showresults As Boolean = False

        Select Case showtranslatorresults
            Case "y"
                showresults = True
            Case "Y"
                showresults = True
        End Select

        If showresults Then

            Dim line_object As LineObject
            Dim tk As TokenCollection
            Dim str As String

            Console.WriteLine("")
            Console.WriteLine("Parser results:")
            Console.WriteLine("")

            For Each line_object In fourth_pass_line_objects ' Debug

                For Each tk In line_object.token_list

                    str = String.Empty
                    If tk.type = TokenType.IsString Then
                        str = " [STR]"
                    ElseIf tk.type = TokenType.UnParsed Then
                        str = " [UNP]"
                    ElseIf tk.type = TokenType.IsType Then
                        str = " [TYP]"
                    ElseIf tk.type = TokenType.IsKeyword Then
                        str = " [KEY]"
                    ElseIf tk.type = TokenType.IsOperator Then
                        str = " [OPE]"
                    ElseIf tk.type = TokenType.IsName Then
                        str = " [NAM]"
                    ElseIf tk.type = TokenType.ArgStart Then
                        str = " [ARGSTA]"
                    ElseIf tk.type = TokenType.Terminator Then
                        str = " [TER]"
                    ElseIf tk.type = TokenType.ArgDelimiter Then
                        str = " [ARGDEL]"
                    End If
                    If tk.context = 0 Then
                        str += " "
                    ElseIf tk.context <> 0 Then
                        str += "{" & tk.context & "} "
                    End If

                    Console.Write(str & tk.token)
                Next

                Console.WriteLine("")
            Next

            Console.WriteLine("")
            Console.WriteLine("Translator results:")
            Console.WriteLine("")

            For Each line In CPPwriter.final_cpp_lines ' Debug

                Console.WriteLine(line)
            Next

        End If

        Dim Compiler As String = "g++"
        Dim InFile As String = AppDomain.CurrentDomain.BaseDirectory + "out\out.ares.cpp"
        Dim OutFile As String = AppDomain.CurrentDomain.BaseDirectory + "out\out.ares.exe"
        Dim Command As String = "-g " + Chr(34) + InFile + Chr(34) + " -o " + Chr(34) + OutFile + Chr(34) + " -static"

        Try
            File.WriteAllLines(InFile, CPPwriter.final_cpp_lines)
        Catch
            ErrorHandler.PrintError("Critical", "could not write file " & InFile)
            GoTo Ending
        End Try

        Console.WriteLine("")
        Console.WriteLine("Finished translation to C++")

        Try
            Process.Start(Compiler, Command)
        Catch
            ErrorHandler.PrintError("Critical", "a MingW (g++) installation is required with binary compilation.")
            GoTo Ending
        End Try

        Console.WriteLine("")
        Console.WriteLine("Started embeded g++ with command " + Command)

        Console.WriteLine("")
        Console.WriteLine("If no error occured, your executable can be found at:")
        Console.WriteLine(OutFile)

Ending:

        Console.ReadLine()
    End Sub
End Module

Public Class Parser

    Public Shared Sub Init()

        source_lines = File.ReadAllLines(source_file_path)

        ParseFirstPass() ' Separates Strings and non-Strings
        source_lines = Nothing
        ParseSecondPass() ' Separates arguments
        first_pass_line_objects = Nothing
        ParseThirdPass() ' Separates tokens using spaces and formating
        second_pass_line_objects = Nothing
        ParseFourthPass() ' Separates Keywords, Names, Types and Operators
        third_pass_line_objects = Nothing

        Console.WriteLine("")
        Console.WriteLine("Finished ARES parsing")

        If translated_to = "cpp" Then

            Translator.TranslateCppFirstPass()
            CPPwriter.DefineOverheadPrototypes()
            CPPwriter.FinilizeLines()
        End If
    End Sub

    Private Shared Sub ParseFirstPass()

        Dim line As String

        For Each line In Program.source_lines ' For each line in source file

            ' Formating
            Dim temp_line = line.Trim()
            temp_line = Regex.Replace(temp_line, "//.*", "") ' Removes Commented lines
            temp_line = Regex.Replace(temp_line, "\\.*", "")

            Dim parsed_line As New LineObject

            If temp_line = String.Empty Then Continue For
            If temp_line.Contains(ARES.string_indicator) Then ' Does line contain a " ?

                Dim string_join_flip As Boolean = False
                Dim temp_token As String = String.Empty
                Dim joint_string As String = String.Empty

                For Each line_char In temp_line ' For each char in line

                    If line_char = ARES.string_indicator Then ' Is char a " ?

                        string_join_flip = Not string_join_flip ' Activate switch
                        If string_join_flip Then

                            If Not String.IsNullOrEmpty(temp_token.Trim()) Then
                                Dim tc As New TokenCollection ' If start of string, put temp_token to line_object
                                tc.token = temp_token.Trim()
                                tc.type = TokenType.UnParsed
                                parsed_line.token_list.Add(tc)
                                temp_token = String.Empty
                            End If
                        Else

                            Dim tc As New TokenCollection ' If end of string, put joint_string to line_object
                            tc.token = Chr(34) + joint_string + Chr(34)
                            tc.type = TokenType.IsString
                            parsed_line.token_list.Add(tc)
                            joint_string = String.Empty
                        End If
                        Continue For
                    End If

                    If string_join_flip Then
                        joint_string += line_char ' If switch is on, put all chars in joint_token
                    Else
                        temp_token += line_char ' If switch is off, put all chars in temp_token
                    End If
                Next

                If string_join_flip Then ' Close current token

                    Dim tc As New TokenCollection
                    tc.token = Chr(34) + joint_string + Chr(34)
                    tc.type = TokenType.IsString
                    parsed_line.token_list.Add(tc)
                    joint_string = String.Empty
                Else
                    If Not String.IsNullOrEmpty(temp_token.Trim()) Then
                        Dim tc As New TokenCollection
                        tc.token = temp_token.Trim()
                        tc.type = TokenType.UnParsed
                        parsed_line.token_list.Add(tc)
                        temp_token = String.Empty
                    End If
                End If
            Else
                If Not String.IsNullOrEmpty(temp_line.Trim()) Then
                    Dim tc As New TokenCollection ' If does not contain a ", dump line in line_object
                    tc.token = temp_line.Trim()
                    tc.type = TokenType.UnParsed
                    parsed_line.token_list.Add(tc)
                End If
            End If

            first_pass_line_objects.Add(parsed_line)
        Next
    End Sub

    Private Shared Sub ParseSecondPass()

        Dim line_object As New LineObject
        Dim tk As New TokenCollection

        For Each line_object In first_pass_line_objects ' For Each line object

            Dim parsed_line As New LineObject
            Dim is_argument As Boolean = False ' Is list of args ?

            Dim temp_token As String = String.Empty
            Dim argument As String = String.Empty

            Dim arg_context As Short = 0

            For Each tk In line_object.token_list ' For each token collection in line object

                If tk.type = TokenType.UnParsed And tk.token = String.Empty Then Continue For
                If tk.type = TokenType.UnParsed Then ' Make sure token is unparsed ( not string )

                    If tk.token.Contains(ARES.arg_start) Or tk.token.Contains(ARES.arg_end) Or tk.token.Contains(ARES.arg_delimiter) Then ' Does contain '(' ')' ?

                        For Each line_char In tk.token ' For Each character in token

                            If line_char = ARES.arg_start Or line_char = ARES.arg_end Then ' Is char = to arg brackets ?

                                If line_char = ARES.arg_start Then

                                    is_argument = True

                                    If Not String.IsNullOrEmpty(argument) Then
                                        Dim tc As New TokenCollection ' If start of argument brackets, put temp_token to line_object
                                        tc.token = argument.Trim()
                                        tc.type = TokenType.UnParsed
                                        tc.context = arg_context
                                        parsed_line.token_list.Add(tc)
                                        argument = String.Empty
                                    Else
                                        Dim tc As New TokenCollection ' If start of argument brackets, put temp_token to line_object
                                        tc.token = temp_token.Trim()
                                        tc.type = TokenType.UnParsed
                                        tc.context = arg_context
                                        parsed_line.token_list.Add(tc)
                                        temp_token = String.Empty
                                    End If

                                    arg_context += 1

                                    Dim tc2 As New TokenCollection ' If end of in argument brackets, put current arguments to line_object
                                    tc2.token = String.Empty
                                    tc2.type = TokenType.ArgStart
                                    tc2.context = arg_context
                                    parsed_line.token_list.Add(tc2)

                                ElseIf line_char = ARES.arg_end Then

                                    is_argument = False

                                    If Not String.IsNullOrEmpty(argument) Then
                                        Dim tc As New TokenCollection ' If end of in argument brackets, put current arguments to line_object
                                        tc.token = argument.Trim()
                                        tc.type = TokenType.UnParsed
                                        tc.context = arg_context
                                        parsed_line.token_list.Add(tc)
                                        argument = String.Empty
                                    End If

                                    Dim tc2 As New TokenCollection ' If end of in argument brackets, put current arguments to line_object
                                    tc2.token = String.Empty
                                    tc2.type = TokenType.Terminator
                                    tc2.context = arg_context
                                    parsed_line.token_list.Add(tc2)

                                    arg_context -= 1

                                End If
                                Continue For

                            ElseIf is_argument And line_char = ARES.arg_delimiter Then

                                Dim temp_arg As String = argument.Replace(",", "")

                                If Not String.IsNullOrEmpty(temp_arg) Then
                                    Dim tc As New TokenCollection ' If end of in argument brackets, put current arguments to line_object
                                    tc.token = temp_arg
                                    tc.type = TokenType.UnParsed
                                    tc.context = arg_context
                                    parsed_line.token_list.Add(tc)
                                    argument = String.Empty
                                End If

                                Dim tc2 As New TokenCollection ' If end of in argument brackets, put current arguments to line_object
                                tc2.token = String.Empty
                                tc2.type = TokenType.ArgDelimiter
                                tc2.context = arg_context
                                parsed_line.token_list.Add(tc2)
                            Else

                                If is_argument Then
                                    argument += line_char ' If switch is on, put all chars in joint_token
                                Else
                                    temp_token += line_char ' If switch is off, put all chars in temp_token
                                End If
                            End If
                        Next

                        If is_argument Then

                            If Not String.IsNullOrEmpty(argument) Then
                                Dim tc As New TokenCollection ' If start of string, put temp_token to line_object
                                tc.token = argument.Trim()
                                tc.type = TokenType.UnParsed
                                tc.context = arg_context
                                parsed_line.token_list.Add(tc)
                                argument = String.Empty
                            End If
                        Else

                            If Not String.IsNullOrEmpty(temp_token) Then
                                Dim tc As New TokenCollection ' If start of string, put temp_token to line_object
                                tc.token = temp_token.Trim()
                                tc.type = TokenType.UnParsed
                                tc.context = arg_context
                                parsed_line.token_list.Add(tc)
                                temp_token = String.Empty
                            End If
                        End If
                    Else

                        If is_argument Then
                            Dim tc As New TokenCollection
                            tc.token = tk.token
                            tc.type = tk.type
                            tc.context = arg_context
                            parsed_line.token_list.Add(tc)
                        Else
                            parsed_line.token_list.Add(tk)
                        End If
                    End If
                Else

                    If is_argument Then
                        Dim tc As New TokenCollection
                        tc.token = tk.token
                        tc.type = tk.type
                        tc.context = arg_context
                        parsed_line.token_list.Add(tc)
                    Else
                        parsed_line.token_list.Add(tk)
                    End If
                End If
            Next

            second_pass_line_objects.Add(parsed_line)
        Next
    End Sub

    Private Shared Sub ParseThirdPass()

        For Each line_object In second_pass_line_objects ' For Each line object

            Dim TempLineObject As LineObject = New LineObject()
            Dim TempTK As TokenCollection = New TokenCollection()

            For Each tk In line_object.token_list ' For each tk in line object

                If tk.type = TokenType.UnParsed Then

                    Dim temp_token = tk.token
                    ' Formating
                    For Each ARES_op In ARES.operators

                        If tk.token.Contains(ARES_op) Then
                            temp_token = temp_token.Replace(ARES_op, ARES.token_delimiter + ARES_op + ARES.token_delimiter)
                        End If
                    Next

                    Dim split_tokens() As String = temp_token.Split(ARES.token_delimiter)

                    For Each token In split_tokens

                        If Not String.IsNullOrWhiteSpace(token) Then
                            TempTK = New TokenCollection()
                            TempTK.token = token
                            TempTK.type = TokenType.UnParsed
                            TempTK.context = tk.context
                            TempLineObject.token_list.Add(TempTK)
                        End If
                    Next
                Else

                    TempLineObject.token_list.Add(tk)
                End If
            Next

            third_pass_line_objects.Add(TempLineObject)
        Next
    End Sub

    Private Shared Sub ParseFourthPass()

        Dim temp_token = String.Empty
        Dim TempTK As New TokenCollection
        Dim TempLO As New LineObject

        For Each line_object In third_pass_line_objects ' For Each line object

            TempLO = New LineObject()

            For Each tk In line_object.token_list

                temp_token = tk.token.Trim()
                TempTK = New TokenCollection()
                TempTK.token = temp_token
                TempTK.context = tk.context

                If tk.type = TokenType.UnParsed Then

                    If ARES.types.Contains(temp_token) Then ' is Type

                        TempTK.type = TokenType.IsType

                    ElseIf ARES.keywords.Contains(temp_token) Then ' is Keyword

                        TempTK.type = TokenType.IsKeyword

                    ElseIf ARES.operators.Contains(temp_token) Then ' is Operator

                        TempTK.type = TokenType.IsOperator

                    Else                                            ' Is Name/Value

                        TempTK.type = TokenType.IsName
                    End If
                Else

                    TempTK.token = temp_token
                    TempTK.type = tk.type
                    TempTK.context = tk.context
                End If

                TempLO.token_list.Add(TempTK)
            Next

            fourth_pass_line_objects.Add(TempLO)
        Next
    End Sub
End Class

Public Class Translator

    Public Shared Sub TranslateCppFirstPass()

        Dim tk_list As List(Of TokenCollection)
        Dim temp_arg_list As New List(Of TokenCollection)
        Dim line_counter As Long = 0

        CPPwriter.InitalHeaders()

        For Each line_object In fourth_pass_line_objects ' For Each line

            line_counter += 1
            temp_arg_list = Nothing
            tk_list = line_object.token_list

            If tk_list(0).type = TokenType.IsKeyword Then ' Is Keyword

                If tk_list(0).token = ARES.kw_function Then ' Is Function declaration

                    Dim return_type As String = ""
                    temp_arg_list = Helper.GetLineTokens(tk_list, 2, 0)

                    If tk_list(tk_list.Count - 2).token = ARES.kw_returns_type Then

                        return_type = tk_list(tk_list.Count - 1).token
                    End If

                    CPPwriter.DeclareFunction(tk_list(1).token, temp_arg_list, return_type)

                ElseIf tk_list(0).token = ARES.kw_end Then ' Is End declaration

                    CPPwriter.DeclaredEnd()

                ElseIf tk_list(0).token = ARES.kw_object Then ' Is End declaration

                    CPPwriter.DeclareObject(tk_list(1).token)

                ElseIf tk_list(0).token = ARES.kw_return Then ' Is End declaration

                    CPPwriter.DeclareReturn()

                ElseIf tk_list(0).token = ARES.kw_let Then ' Is variable assignment

                    '
                    '   TO IMPLEMENT
                    '
                    ErrorHandler.PrintError("Unsupported", "ARES complier does not currently support variable assignment.", line_counter)

                ElseIf tk_list(0).token = ARES.kw_call Then ' Is Function call

                    temp_arg_list = Helper.GetLineTokens(tk_list, 2, tk_list(1).context)
                    CPPwriter.CallFunction(tk_list(1).token, temp_arg_list)
                End If
            ElseIf tk_list(0).type = TokenType.IsType Then ' Is variable assignment / variable declaration

                CPPwriter.DeclareVariable(tk_list(1).token, tk_list(0).token)

            ElseIf tk_list(0).type = TokenType.IsName Then

                If CPPwriter.cpp_declared_variables.ContainsKey(tk_list(0).token) Then

                    '
                    '   TO IMPLEMENT
                    '
                    ErrorHandler.PrintError("Unsupported", "ARES complier does not currently support variable assignment.", line_counter)
                Else

                    temp_arg_list = Helper.GetLineTokens(tk_list, 1, tk_list(0).context)
                    CPPwriter.CallFunction(tk_list(0).token, temp_arg_list)
                End If
            ElseIf tk_list(0).type = TokenType.IsOperator Then

                ErrorHandler.PrintError("Syntax", "Unexpected operator.", line_counter)
            End If
        Next
    End Sub
End Class

Public Class Helper

    Public Shared Function GetLineTokens(ByRef token_list As List(Of TokenCollection), Optional ByVal start_index As Integer = 1, Optional ByVal current_context As Short = 0) As List(Of TokenCollection)

        Dim ReturnArray As New List(Of TokenCollection)

        If start_index >= token_list.Count Then Return ReturnArray

        Dim found_arg_start As Boolean = False

        For i = start_index To token_list.Count - 1

            If token_list(i).type = TokenType.ArgStart Then
                found_arg_start = True
                Exit For
            End If
        Next i

        If found_arg_start Then

            For i = start_index To token_list.Count - 1

                ReturnArray.Add(token_list(i))

                If token_list(i).type = TokenType.Terminator Then

                    If token_list(i).context - 1 = 0 Then

                        Exit For
                    End If
                End If
            Next i
        Else

            Dim tc As New TokenCollection
            tc.token = String.Empty
            tc.type = TokenType.ArgStart
            tc.context = current_context + 1
            ReturnArray.Add(tc)

            tc.type = TokenType.Terminator
            ReturnArray.Add(tc)
        End If
        Return ReturnArray
    End Function
End Class

Public Class TokenCollection

    Public Property token As String = String.Empty
    Public Property type As TokenType = TokenType.UnParsed
    Public Property context As Short = 0
End Class

Public Class LineObject

    Public Property token_list As New List(Of TokenCollection)()
End Class

Public Enum TokenType

    UnParsed
    IsName
    IsKeyword
    IsType
    IsOperator
    IsString

    ArgStart
    ArgDelimiter
    Terminator
End Enum

Public Class ErrorHandler

    Public Shared Sub PrintError(ByRef context As String, ByRef description As String, Optional ByRef line As Long = -1)

        If line = -1 Then
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.WriteLine("/!\ ERROR /!\ : " & context & " error, " & description)
            Console.WriteLine("----------------------------------------------------------------------------------")
        Else
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.WriteLine("/!\ ERROR /!\ : " & context & " error, at line " & line & " : " & description)
            Console.WriteLine("----------------------------------------------------------------------------------")
        End If
    End Sub
End Class