# Introduction to the ARES language

The ARES language is a programation language created by Jérémie Brossard in may of 2022.

What this language aims to be(come) is a language focused on simplicity, productivity and performance, a combination of ideals too rarely obtained in a single language.

The language in itself can be described as simple, understandable, with abreviated keywords and plain english standard methods / objects.

## Guiding principles

Simplicity of writing

Simplicity of comprehension

No compromising

No quirks, no bs

### Simplicity of writing

Repeated keywords and data type names are short and abbreviated in a way that makes them easy to write. ARES breaks down the format of the code to its fundamentals, which leads to less time thinking and caring about details will negatively affect productivity and creativity.

### Simplicity of comprehension

Keywords and standard methods are explicitly named and act as the user thinks it should act. Multiple methods can lead to the same result, but this diversity of choice is meaningful in a dev's ability to be creative and productive.

### No compromising

Simplicity should not come at the cost of performances. What ARES does is to not reinvent the wheel: it takes from the bests to give the best back to its users.

### No quirks, no bs

ARES and its standard methods should act exactly like the user thinks it should act. Any quirk is another barrier to entry, barrier to learning, barrier to enjoying coding. Behavior should be standard across the whole medium.

Anything that is unnecessary should not be there in the first place.

## ARES Syntax

*As of 2022/04/23*

Legend:
[Variable]
(Optional)

### Function / method declaration

```
Func [function name] ( '(' [argument list] ')' ) (>> [return data type]) ... End / Return

ex:
// Definition of the program's entry point
Func main
  // Code
  End

// Definition of a method with arguments and a return type
Func ReturnsInt(Int x, Str y) >> Int
  // Code
  Return z
```

### Variable declaration
```
[data type] [variable name]

ex:
Str StringVariable
```

### Object declaration
```
Obj [object name] ... End

ex:
Obj ThisIsAnObject
  // Code
  End
```

## ARES Data types

and their C++ equivalences

*As of 2022/04/23*

### String
`Str` `std::string`

### String Array
`Str[]` `std::vector<string>`

### Integer
`Int` `long int`

### Long
`Lng` `long long int`

### Short
`Sht` `int`

### Double
`Dbl` `double`

### Decimal
`Deci` `long double`

### Float
`Flt` `float`

### Boolean
`Bol` `bool`

## ARES Standard methods

*As of 2022/04/23*

### Print
Writes to the console

`Print(Var input)`
```
Print("Hello, World!") // Outputs Hello, World
```
### Replace
Replaces all instances of a string or character inside a string with another.

`Replace( Str in_string, Str look_for, Str replace_with) >> Str`
```
Replace("xHexllxo,x Worxldx!", "x", "") // Replaces all "x" with "", result is "Hello, World!"
Replace("This is Hello, World!", "This is ", "") // Replaces "This is" with "", result is "Hello, World!"
Replace("x", "x", "Hello, World!") // Replaces "x" with "Hello, World!", result is "Hello, World!"
```
### Split
Splits a string into an string array using a delimiter.

`Split( Str in_string, Str delimiter) >> Str[]`
```
Split("Hello, World!", ",") // Result is ["Hello"," World!"]
```
### GetToken
Returns a string from another using a delimiter and an index.

`GetToken( Str in_string, Str delimiter, Lng index) >> Str`
```
GetToken("discarded;Hello, World!;discarded", ";", 2) // Result is "Hello, World!"
```
### Concat
Concatenates two strings.

`Concat( Str left, Str right) >> Str`
```
Concat("Hello, ", "World!")
```
### Append
Appends a value to a string.

`Append( Str left, Var right) >> Str`
```
Append("Hello, World!", 0) // Result is "Hello, World!0"
Append("Hello, ", "World!") // Result is "Hello, World!"
```
### ToStr
Converts value to string.

`ToStr( Var input) >> Str`
```
ToStr(50) // Result is "50"
```
### SubStr
Gets a string using n characters from another, with an offset.

`SubStr( Str in_string, Lng offset, Lng length) >> Str`
```
SubStr("OverHello, World!flow", 4 ,13)
```
### Left
Gets a string using n characters from the left of another.

`Left( Str in_string, Lng length) >> Str`
```
Left("Hello, World!discarded", 13) // Result is "Hello, World!"
```
### Right
Gets a string using n characters from the right of another.

`Right( Str in_string, Lng length) >> Str`
```
Right("discardedHello, World!", 13) // Result is "Hello, World!"
```
### Find
Returns the position of the first match of a string inside another.

`Find( Str in_string, Str to_find) >> Lng`
```
Find("Hello, World!", "W") // Returns 8
```
### FindCount
Returns the number of matches of a string inside another.

`FindCount( Str in_string, Str to_find) >> Lng`
```
FindCount("Hello, World", "l") // Returns 3
```
### Length
Returns the number of characters inside a string.

`Length( Str input) >> Lng`
```
Length("Hello, World!") // Returns 13
```
### Add
Adds a value to another.

`Add( Dbl left, Var right) >> Dbl`

`Add( Str left, Var right) >> Str`
```
Add(50, 5) // Result is 55
Add("Hello,", " World!") // Result is "Hello, World!"
```
### Substract
Substracts two numerical values.

`Substract( Dbl left, Var right) >> Dbl`
```
Substract(500, 250) // Result is 250
```
### Multiply
Multiplies two numerical values.

`Multiply( Dbl left, Var right) Dbl`
```
Multiply( 5, 2) // Result is 10
```
### Divide
Divides two numerical values.

`Divide( Dbl left, Var right) Dbl`
```
Divide( 10, 2) // Result is 5
```
### Pow
Raises a numerical value to another.

`Pow( Dbl left, Var right) Dbl`
```
Pow( 10, 2) // Result is 100
```
### Round

Round( Dbl, Lng ) >> Dbl
Max( Dbl, Dbl ) >> Dbl
Min( Dbl, Dbl ) >> Dbl
Average( Dbl, Dbl ) >> Dbl
Random( Dbl, Dbl, Lng ) >> Dbl

// Bol
Flip( Bol ) >> Bol

You can find more exemples at https://github.com/JeremieOverGs/ARES-Compiler/tree/main/ARES%20Transpiler/ARES%20snippets
