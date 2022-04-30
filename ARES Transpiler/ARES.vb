﻿Public Class ARES

    Public Const string_indicator As Char = Chr(34)
    Public Const arg_start As Char = "("
    Public Const arg_end As Char = ")"
    Public Const arg_delimiter As Char = ","
    Public Const token_delimiter As Char = " "

    Public Const kw_function As String = "Func"
    Public Const kw_end As String = "End"
    Public Const kw_return As String = "Return"
    Public Const kw_if As String = "If"
    Public Const kw_else As String = "Else"
    Public Const kw_for As String = "For"
    Public Const kw_while As String = "While"
    Public Const kw_returns_type As String = ">>"
    Public Const kw_object As String = "Obj"
    Public Const kw_let As String = "Let"
    Public Const kw_call As String = "Call"

    Public Shared types As List(Of String) = New List(Of String) From {
        "Str",
        "Int",
        "Sht",
        "Lng",
        "Dbl",
        "Dci",
        "Flt",
        "Bol",
        "Nul"}

    Public Shared typesToCPP As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
        {"Str", "std::string"},
        {"Int", "long int"},
        {"Sht", "int"},
        {"Lng", "long long int"},
        {"Dbl", "double"},
        {"Dci", "long double"},
        {"Flt", "float"},
        {"Bol", "bool"},
        {"Nul", "void"}}

    Public Shared keywords As List(Of String) = New List(Of String) From {
        "Func",
        "End",
        "Return",
        "If",
        "Else",
        "For",
        "While",
        "Let",
        "Call",
        ">>",
        "Obj"}

    Public Shared operators As List(Of String) = New List(Of String) From {
        "+",
        "-",
        "=",
        "/",
        "^",
        "*"}

    Public Shared standard_functions As List(Of String) = New List(Of String) From {
        "Print",
        "Split",
        "GetToken",
        "Replace",
        "Append",
        "Concat",
        "SubStr",
        "Left",
        "Right",
        "Length",
        "Find",
        "FindCount",
        "ToStr",
        "Add",
        "Substract",
        "Multiply",
        "Divide",
        "Pow",
        "Round",
        "Max",
        "Min",
        "Average",
        "Random",
        "Flip",
        "Input"}

    Public Shared standard_objects As List(Of String) = New List(Of String) From {
        "System",
        "Application"}
End Class
