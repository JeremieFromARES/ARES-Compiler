Public Class ARES

    Public Const string_indicator As Char = Chr(34)
    Public Const arg_start As Char = "("
    Public Const arg_end As Char = ")"
    Public Const arg_delimiter As Char = ","
    Public Const token_delimiter As Char = " "

    Public Const kw_function As String = "Fn"
    Public Const kw_end As String = "End"
    Public Const kw_return As String = "Return"
    Public Const kw_if As String = "If"
    Public Const kw_else As String = "Else"
    Public Const kw_loop As String = "Loop"
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
        "Nul",
        "Str[]",
        "Int[]",
        "Sht[]",
        "Lng[]",
        "Dbl[]",
        "Dci[]",
        "Flt[]",
        "Bol[]"}

    Public Shared typesToCPP As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
        {"Str", "ARES::Types::String"},
        {"Int", "long int"},
        {"Sht", "int"},
        {"Lng", "long long int"},
        {"Dbl", "double"},
        {"Dci", "long double"},
        {"Flt", "float"},
        {"Bol", "bool"},
        {"Str[]", "ARES::Types::Array<ARES::Types::String>"},
        {"Int[]", "ARES::Types::Array<long int>"},
        {"Sht[]", "ARES::Types::Array<int>"},
        {"Lng[]", "ARES::Types::Array<long long int>"},
        {"Dbl[]", "ARES::Types::Array<double>"},
        {"Dci[]", "ARES::Types::Array<long double>"},
        {"Flt[]", "ARES::Types::Array<float>"},
        {"Bol[]", "ARES::Types::Array<bool>"},
        {"Nul", "void"}}

    Public Shared keywords As List(Of String) = New List(Of String) From {
        "Fn",
        "End",
        "Return",
        "If",
        "Else",
        "Loop",
        "Let",
        "Call",
        ">>",
        "Obj",
        "Not",
        "And",
        "Or"}

    Public Shared operators As List(Of String) = New List(Of String) From {
        "+",
        "-",
        "=",
        "/",
        "^",
        "*",
        "!"}

    Public Shared standard_functions As List(Of String) = New List(Of String) From {
        "Print",
        "Input",
        "Split",
        "Replace",
        "Append",
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
        "RandomBol",
        "Flip",
        "Now",
        "Date",
        "DateToStr",
        "Str",
        "Insert",
        "Delete",
        "Contains",
        "Count",
        "IndexOf",
        "Length",
        "ExecTime",
        "RemoveIndex"}

    Public Shared standard_objects As List(Of String) = New List(Of String) From {
        "Sys",
        "Time",
        "Debug",
        "App",
        "CPP"}
End Class
