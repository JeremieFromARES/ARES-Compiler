// using the ARES language 2022, under GPL3 license.
// Transpile date: 2022/57/23 10:57:02

// ARES - Headers
#include "ARES.h"

// ARES - Overhead prototypes
int main();
std::string StringManipulation( std::string x1);
long int NumberManipulation( long int x1);

// ARES - Program
int main() {
StringManipulation( "Hello, World!");
NumberManipulation(0);
ARES::Input();
};
std::string StringManipulation(   std::string input) {
std::string __ARES_return__;
ARES::Print( "");
ARES::Print( "String Manipulation");
ARES::Print( "");
ARES::Print( ARES::Replace( "xHexllxo,x Worxldx!", "x", ""));
ARES::Print( ARES::Replace( "x", "x", "Hello, World!"));
ARES::Print( ARES::Replace( "This is Hello, World!", "This is ", ""));
ARES::Print( ARES::GetToken( "Tokenized;Hello, World!;WOW!", ";",2));
ARES::Print( ARES::Append( "Hello, ", "World!"));
ARES::Print( ARES::Concat( "Hello, ", "World!"));
ARES::Print( ARES::SubStr( "OverHello, World!flow",4,13));
ARES::Print( ARES::Left( "Hello, World!Overflow",13));
ARES::Print( ARES::Right( "OverflowHello, World!",13));
ARES::Print( ARES::Length( "Hello, World!"));
ARES::Print( ARES::Find( "Hello, World!", "W"));
ARES::Print( ARES::FindCount( "Hello, World", "l"));
return __ARES_return__;
};
long int NumberManipulation(   long int input) {
long int __ARES_return__;
ARES::Print( "");
ARES::Print( "Number Manipulation");
ARES::Print( "");
ARES::Print( ARES::Add(0,5));
ARES::Print( ARES::Add(0.2,0.7));
ARES::Print( ARES::Add(0.2,23));
ARES::Print( ARES::Substract(500,250));
ARES::Print( ARES::Substract(250.25,0.10));
ARES::Print( ARES::Substract(250,50.50));
ARES::Print( ARES::Pow(10,2));
ARES::Print( ARES::Pow(10,2.5));
ARES::Print( ARES::Round(0.556,1));
ARES::Print( ARES::Max(0.5,1));
ARES::Print( ARES::Min(0.5,1));
ARES::Print( ARES::Average(0,1));
ARES::Print( ARES::Random(0,1));
ARES::Print( ARES::Random(50,100));
ARES::Print( ARES::Random(0,1,500));
return __ARES_return__;
};
