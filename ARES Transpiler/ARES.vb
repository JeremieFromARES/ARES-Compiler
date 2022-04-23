Public Class ARES

    Public Shared Property string_indicator As Char = Chr(34)
    Public Shared Property arg_start As Char = "("
    Public Shared Property arg_end As Char = ")"
    Public Shared Property arg_delimiter As Char = ","
    Public Shared Property token_delimiter As Char = " "

    Public Shared Property kw_function As String = "Func"
    Public Shared Property kw_end As String = "End"
    Public Shared Property kw_return As String = "Return"
    Public Shared Property kw_if As String = "If"
    Public Shared Property kw_else As String = "Else"
    Public Shared Property kw_for As String = "For"
    Public Shared Property kw_while As String = "While"
    Public Shared Property kw_returns_type As String = ">>"
    Public Shared Property kw_object As String = "Obj"
    Public Shared Property kw_let As String = "Let"
    Public Shared Property kw_call As String = "Call"

    Public Shared Property types As List(Of String) = New List(Of String) From {
        "Str",
        "Int",
        "Sht",
        "Lng",
        "Dbl",
        "Dci",
        "Flt",
        "Bol",
        "Nul"}

    Public Shared Property typesToCPP As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
        {"Str", "std::string"},
        {"Int", "long int"},
        {"Sht", "int"},
        {"Lng", "long long int"},
        {"Dbl", "double"},
        {"Dci", "long double"},
        {"Flt", "float"},
        {"Bol", "bool"},
        {"Nul", "void"}}

    Public Shared Property keywords As List(Of String) = New List(Of String) From {
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

    Public Shared Property operators As List(Of String) = New List(Of String) From {
        "+",
        "-",
        "=",
        "/",
        "^",
        "*"}

    Public Shared Property standard_functions As List(Of String) = New List(Of String) From {
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
        "Flip"}
End Class
