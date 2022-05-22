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

    Public Sub Main(ByVal args() As String)

        Dim IsFromCL As Boolean = (args.Count <> 0)
        Dim showtranslatorresults As String

        Console.ForegroundColor = ConsoleColor.DarkGray
        Console.WriteLine("ARES Transpiler - a1.0.8")
        Console.ForegroundColor = ConsoleColor.Blue
        Console.WriteLine("            ___   ____  ____")
        Console.WriteLine("   -|- //| || \\ ||___ //__ ")
        Console.WriteLine("      //|| ||_// ||       \\")
        Console.WriteLine("     // || || \\ ||___ \\_//")
        Console.ForegroundColor = ConsoleColor.DarkGray

        Dim Compiler As String = "g++"
        Dim InFile As String = AppDomain.CurrentDomain.BaseDirectory + "out\out.ares.cpp"
        Dim OutFile As String = AppDomain.CurrentDomain.BaseDirectory + "out\out.ares.exe"

        If IsFromCL Then

            source_file_path = args(0)
            OutFile = args(1)
        End If

        Dim Command As String = "-g " + Chr(34) + InFile + Chr(34) + " -o " + Chr(34) + OutFile + Chr(34) + " -static"

        If Not IsFromCL Then

            Console.WriteLine("")
            Console.WriteLine("")
            Console.Write("ARES source code (")
            Console.ForegroundColor = ConsoleColor.Blue
            Console.Write(".ares")
            Console.ForegroundColor = ConsoleColor.DarkGray
            Console.Write(") absolute path:" & vbNewLine)
            Console.ForegroundColor = ConsoleColor.Black
            Console.BackgroundColor = ConsoleColor.Gray
            source_file_path = Console.ReadLine().Replace("\", "/").Replace("¥", "/").Replace("""", "")
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.DarkGray

            Select Case source_file_path ' Code Snippets
                Case "0"
                    source_file_path = AppDomain.CurrentDomain.BaseDirectory & "ARES snippets\ARES_test_code.ares"
                Case "1"
                    source_file_path = AppDomain.CurrentDomain.BaseDirectory & "ARES snippets\helloworld.ares"
                Case "2"
                    source_file_path = AppDomain.CurrentDomain.BaseDirectory & "ARES snippets\operator_testing.ares"
            End Select

            Console.WriteLine("Show parser/transpiler results? (debug purposes). [Y/n]")
            Console.ForegroundColor = ConsoleColor.Black
            Console.BackgroundColor = ConsoleColor.Gray
            showtranslatorresults = Console.ReadLine()
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If

        Parser.Init()

        If Not IsFromCL Then

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
                Dim ln_cn As Long

                Console.WriteLine("")
                Console.WriteLine("Parser results")
                Console.WriteLine("")

                For Each line_object In fourth_pass_line_objects ' Debug

                    ln_cn += 1
                    Console.Write(ln_cn & " ")

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
                            str = " [ARGTER]"
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
                Console.WriteLine("Translator results")
                Console.WriteLine("")

                For Each line In CPPwriter.final_cpp_lines ' Debug

                    Console.WriteLine(line)
                Next

                'Console.WriteLine("")
                'For Each line In third_pass_line_objects ' Debug

                '    For Each token In line.token_list
                '        Console.Write(token.token + " ")
                '    Next
                '    Console.WriteLine("")
                'Next

            End If

        End If

        File.Delete(OutFile)

        If ErrorHandler.ErrorCounter > 0 Then

            Console.WriteLine("")
            ErrorHandler.PrintWarning("Error checking", $"terminated translation due to {ErrorHandler.ErrorCounter} errors.")
            GoTo Ending
        End If

        Try
            File.WriteAllLines(InFile, CPPwriter.final_cpp_lines)
        Catch
            ErrorHandler.PrintError("Critical", "could Not write file " & InFile)
            GoTo Ending
        End Try

        Console.WriteLine("# Finished translation to intermediate C++.")

        Console.WriteLine("# Started g++ with command " + Command)

        Try
            Process.Start(Compiler, Command)
        Catch
            Debug.Print("! Compiler : Error due to missing MingW/MingW_64 installation. Note that this issue can also occur due to missing system variable to the MingW's directory.")
            ErrorHandler.PrintError("Critical", "a MingW/MingW_64 (g++) installation is required for executable compilation." & Environment.NewLine & "If MingW is already installed please make sure that you have correctly setup the system variable pointing to it's directory")
            GoTo Ending
        End Try

        If Not File.Exists(OutFile) Then
            Threading.Thread.Sleep(2500)
            If Not File.Exists(OutFile) Then
                Debug.Print("! Compiler : Error due to unhandled error while translating. Please refer to C:/ARES/out/out.ares.cpp to debug manually.")
                ErrorHandler.PrintError("Critical", "g++ could not finish compilation due to unhandled ARES error.")
                GoTo Ending
            End If
        End If
        Console.WriteLine("# Compilation completed.")
        Debug.Print("# Compiler - Finished Compilation")

        Console.WriteLine("")
        Console.WriteLine("The compiled executable file can be found at:")
        Console.ForegroundColor = ConsoleColor.Black
        Console.BackgroundColor = ConsoleColor.Gray
        Console.WriteLine(OutFile)
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.DarkGray
        Console.WriteLine(" ")
Ending:
        Debug.Print("# ARES - Program Ended - Waiting for input.")
        If Not IsFromCL Then
            Console.ReadLine()
        End If
    End Sub
End Module

Public Class Parser

    Public Shared Sub Init()

        source_lines = File.ReadAllLines(source_file_path)

        Debug.Print("# Parse - First pass ...")
        ParseFirstPass() ' Separates Strings and non-Strings
        source_lines = Nothing
        Debug.Print("# Parse - Second pass ...")
        ParseSecondPass() ' Separates arguments
        ' first_pass_line_objects = Nothing
        Debug.Print("# Parse - Third pass ...")
        ParseThirdPass() ' Separates tokens using spaces and formating
        ' second_pass_line_objects = Nothing
        Debug.Print("# Parse - Fourth pass ...")
        ParseFourthPass() ' Separates Keywords, Names, Types and Operators
        ' third_pass_line_objects = Nothing

        Console.WriteLine("")
        Console.WriteLine("# Finished ARES parsing")

        If translated_to = "cpp" Then

            Debug.Print("# Translator - First pass ...")
            Translator.TranslateCppFirstPass()
            Debug.Print("# Translator - Second pass ...")
            Translator.TranslateCppSecondPass()
            Debug.Print("# Translator - Finilizing ...")
            CPPwriter.DefineOverheadPrototypes()
            CPPwriter.FinilizeLines()
            Debug.Print("# Compiler - Compilation ...")
        End If
    End Sub

    Private Shared Sub ParseFirstPass()

        Dim line As String

        For Each line In Program.source_lines ' For each line in source file

            If line.Contains("§$§@#") Then

                ErrorHandler.PrintError("Syntax", "the term ""§$§@#"" is reserved by the translator.")
            End If

            ' Formating
            Dim temp_line = line.Trim()
            temp_line = Regex.Replace(temp_line, "//.*", "") ' Removes Commented lines
            temp_line = Regex.Replace(temp_line, "\\\\.*", "")
            temp_line = Regex.Replace(temp_line, "\\""", "§$§@#")

            Dim parsed_line As New LineObject

            'If temp_line = String.Empty Then Continue For
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

                                    ' TEMPORARY
                                    If Not String.IsNullOrEmpty(temp_token) Then
                                        Dim tc As New TokenCollection
                                        tc.token = temp_token.Trim()
                                        tc.type = TokenType.UnParsed
                                        tc.context = arg_context
                                        parsed_line.token_list.Add(tc)
                                        temp_token = String.Empty
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
                                Dim tc As New TokenCollection
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

                    If tk.type = TokenType.IsString Then
                        tk.token = tk.token.Replace("§$§@#", "\""")
                    End If

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

            Dim i As Long = 0

            For i = 0 To line_object.token_list.Count - 1

                Dim tk = line_object.token_list(i)

                temp_token = tk.token.Trim()
                TempTK = New TokenCollection()
                TempTK.token = temp_token
                TempTK.context = tk.context

                Dim next_token As New TokenCollection()
                Try
                    next_token = line_object.token_list(i + 1)
                Catch

                End Try

                If tk.type = TokenType.UnParsed Then

                    If ARES.types.Contains(temp_token) And next_token.type <> TokenType.ArgStart Then ' is Type

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

    Public Shared line_counter As Long = 0
    Public Shared object_def As Boolean = False
    Public Shared indentation As Integer = 0

    Public Shared Sub TranslateCppFirstPass()

        line_counter = 0
        object_def = False
        indentation = 0

        Dim tk_list As List(Of TokenCollection)

        For Each line_object In fourth_pass_line_objects ' For Each line

            line_counter += 1
            tk_list = line_object.token_list

            If tk_list.Count = 0 Then Continue For

            If tk_list(0).type = TokenType.IsKeyword Then ' Is Keyword

                If tk_list(0).token = ARES.kw_function Then ' Is Function declaration

                    CPPwriter.PreDeclareFunction(tk_list(1).token)

                ElseIf tk_list(0).token = ARES.kw_object Then ' Is End declaration

                    CPPwriter.PreDeclareObject(tk_list(1).token)
                End If
            ElseIf tk_list(0).type = TokenType.IsType Or CPPwriter.cpp_declared_objects.Contains(tk_list(0).token) Then ' Is variable assignment / variable declaration

                CPPwriter.PreDeclareVariable(tk_list(1).token)

            End If
        Next
    End Sub

    Public Shared Sub TranslateCppSecondPass()

        line_counter = 0
        object_def = False
        indentation = 0

        Dim tk_list As List(Of TokenCollection)
        Dim temp_arg_list As New List(Of TokenCollection)

        CPPwriter.InitalHeaders()

        For Each line_object In fourth_pass_line_objects ' For Each line

            line_counter += 1
            temp_arg_list = Nothing
            tk_list = line_object.token_list

            If tk_list.Count = 0 Then Continue For

            If tk_list(0).type = TokenType.IsKeyword Then ' Is Keyword

                If tk_list(0).token = ARES.kw_function Then ' Is Function declaration

                    Dim return_type As String = ""
                    temp_arg_list = Helper.GetFuncArgs(tk_list, 2, 0)

                    If tk_list(tk_list.Count - 2).token = ARES.kw_returns_type Then

                        return_type = tk_list(tk_list.Count - 1).token
                    End If

                    If ARES.typesToCPP.Keys.Contains(tk_list(tk_list.Count - 1).token) Then
                        return_type = tk_list(tk_list.Count - 1).token
                    End If

                    CPPwriter.DeclareFunction(tk_list(1).token, temp_arg_list, return_type)

                ElseIf tk_list(0).token = ARES.kw_end Then ' Is End declaration

                    CPPwriter.DeclaredEnd()

                ElseIf tk_list(0).token = ARES.kw_object Then ' Is Object declaration

                    CPPwriter.DeclareObject(tk_list(1).token)

                ElseIf tk_list(0).token = ARES.kw_return Then ' Is Return declaration

                    CPPwriter.DeclareReturn(Helper.GetLineTokens(tk_list, 1, 0))

                ElseIf tk_list(0).token = ARES.kw_let Then ' Is variable assignment

                    CPPwriter.AssignToVariable(tk_list(1).token, Helper.GetLineTokens(tk_list, 2, 0))

                ElseIf tk_list(0).token = ARES.kw_call Then ' Is Function call

                    temp_arg_list = Helper.GetFuncArgs(tk_list, 2, tk_list(1).context)
                    CPPwriter.CallFunction(tk_list(1).token, temp_arg_list)

                ElseIf tk_list(0).token = ARES.kw_loop Then

                    Dim linetokens As New List(Of TokenCollection)
                    linetokens = Helper.GetLineTokens(tk_list, 1, 0)
                    CPPwriter.DeclareLoop(linetokens)

                ElseIf tk_list(0).token = ARES.kw_if Or tk_list(0).token = ARES.kw_else Then

                    Dim temp_prefix As String = ""
                    Dim linetokens As New List(Of TokenCollection)

                    If tk_list(0).token = ARES.kw_else Then
                        temp_prefix = "} else "
                    End If

                    If tk_list.Count > 1 Then
                        If tk_list(1).type = TokenType.IsKeyword Then

                            linetokens = Helper.GetLineTokens(tk_list, 2, 0)
                        Else

                            linetokens = Helper.GetLineTokens(tk_list, 1, 0)
                        End If
                    End If

                    If linetokens.Count <= 1 Then

                        If tk_list(0).token = ARES.kw_if Then

                            ErrorHandler.PrintError("Syntax", "Expected condition after If.", line_counter)
                        Else

                            CPPwriter.DeclareIf(New List(Of TokenCollection), temp_prefix)
                        End If
                    Else

                        CPPwriter.DeclareIf(linetokens, temp_prefix)
                    End If
                End If
            ElseIf tk_list(0).type = TokenType.IsType Or CPPwriter.cpp_pre_declared_objects.Contains(tk_list(0).token) Then ' Is variable assignment / variable declaration

                CPPwriter.DeclareVariable(tk_list(1).token, tk_list(0).token, Helper.GetLineTokens(tk_list, 2, 0))

            ElseIf tk_list(0).type = TokenType.IsName Then

                If CPPwriter.cpp_pre_declared_variables.Contains(tk_list(0).token) Then

                    CPPwriter.AssignToVariable(tk_list(0).token, Helper.GetLineTokens(tk_list, 1, 0))

                ElseIf CPPwriter.cpp_pre_declared_objects.Contains(tk_list(0).token) Then


                ElseIf Right(tk_list(0).token, 2) = "++" Then 'Increment variable

                    If CPPwriter.cpp_pre_declared_variables.Contains(Left(tk_list(0).token, tk_list(0).token.Length - 2)) Then
                        CPPwriter.WriteCPP(tk_list(0).token & ";")
                    Else
                        ErrorHandler.PrintError("Syntax", $"Cannot increment {Left(tk_list(0).token, tk_list(0).token.Length - 2)} since it is not defined.", line_counter)
                    End If

                Else

                    temp_arg_list = Helper.GetLineTokens(tk_list, 1, tk_list(0).context) ' TO CHANGE BACK TO getFuncArgs IF NOT WORKING
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

        For i = start_index To token_list.Count - 1

            ReturnArray.Add(token_list(i))

            'If token_list(i).type = TokenType.Terminator Then

            '    If token_list(i).context - 1 = 0 Then

            '        Exit For
            '    End If
            'End If
        Next i

        Return ReturnArray
    End Function

    Public Shared Function GetFuncArgs(ByRef token_list As List(Of TokenCollection), Optional ByVal start_index As Integer = 1, Optional ByVal current_context As Short = 0) As List(Of TokenCollection)

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

    Public Shared Property ErrorCounter As Long = 0

    Public Shared Sub PrintError(ByRef context As String, ByRef description As String, Optional ByRef line As Long = -1)

        ErrorCounter += 1

        If line = -1 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.WriteLine("/!\ ERROR /!\ : " & context & " error, " & description)
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.ForegroundColor = ConsoleColor.DarkGray
        Else
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.WriteLine("/!\ ERROR /!\ : " & context & " error, at line " & line & " : " & description)
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If
    End Sub

    Public Shared Sub PrintWarning(ByRef context As String, ByRef description As String, Optional ByRef line As Long = -1)
        If line = -1 Then
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.WriteLine("/!\ WARNING /!\ : " & context & " warning, " & description)
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.ForegroundColor = ConsoleColor.DarkGray
        Else
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.WriteLine("/!\ WARNING /!\ : " & context & " warning, at line " & line & " : " & description)
            Console.WriteLine("----------------------------------------------------------------------------------")
            Console.ForegroundColor = ConsoleColor.DarkGray
        End If
    End Sub
End Class