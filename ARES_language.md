# Introduction to the ARES language

The ARES language is a programation language created by Jérémie Brossard in may of 2022.

What this language aims to be(come) is a language focused on simplicity, productivity and performance, a combination of ideals too rarely obtained in a single language.

The language in itself can be described as simple, understandable, with abreviated keywords and plain english standard methods / objects.

## Guiding principles

>Simplicity of writing
>
>Simplicity of comprehension
>
>No compromising
>
>No quirks, no bs

### Simplicity of writing

Repeated keywords and data type names are short and abbreviated in a way that makes them easy to write. ARES breaks down the format of the code to its fundamentals, which leads to less time thinking and caring about details will negatively affect productivity and creativity.

### Simplicity of comprehension

Keywords and standard methods are explicitly named and act as the user thinks it should act. Multiple methods can lead to the same result, but this diversity of choice is meaningful in a dev's ability to be creative and productive.

### No compromising

Simplicity should not come at the cost of performances. What ARES does is to not reinvent the wheel: it takes from the bests to give the best back to its users.

### No quirks, no bs

ARES and its standard methods should act exactly like the user thinks it should act. Any quirk is another barrier to entry, barrier to learning, barrier to enjoying coding. Behavior should be standard across the whole medium.

Anything that is unnecessary should not be there in the first place.

## ARES Keywords

*As of 2022/04/23*

>Legend:
>
>[Variable]
>
>(Optional)

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

```
// Global
Print( Var )

// Str
Replace( Str, Str, Str ) >> Str
Split( Str, Str ) >> Str[]
GetToken( Str, Str, Lng ) >> Str
Append( Str, Var ) >> Str
Concat( Str, Str ) >> Str
ToStr( Var ) >> Str
Left( Str, Lng ) Str
Right( Str, Lng ) >> Str
Find( Str, Str ) >> Lng
SubStr( Str, Lng, Lng) >> Str
Length( Str ) >> Lng
FindCount( Str, Str ) >> Lng
Add( Str, Var ) >> Str

// Int/Dbl/Lng/Sht
Add( Dbl, Var ) >> Dbl
Substract( Dbl, Var ) >> Dbl
Multiply( Dbl, Var ) Dbl
Divide( Dbl, Var ) >> Dbl
Pow( Dbl, Var ) >> Dbl
Round( Dbl, Lng ) >> Dbl
Max( Dbl, Dbl ) >> Dbl
Min( Dbl, Dbl ) >> Dbl
Average( Dbl, Dbl ) >> Dbl
Random( Dbl, Dbl, Lng ) >> Dbl

// Bol
Flip( Bol ) >> Bol
```

You can find more exemples at https://github.com/JeremieOverGs/ARES-Compiler/tree/main/ARES%20Transpiler/ARES%20snippets
