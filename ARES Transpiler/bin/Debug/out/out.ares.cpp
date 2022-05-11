// Using the ARES language 2022, under GPL3 license.
// Transpile date: 2022/42/11 09:42:21

// ARES - Headers
#include "ARES.h"

// ARES - Overhead prototypes
int main();

// ARES - Objects

// ARES - Program
int main() {
ARES::__ARES_MAIN_INIT__();
ARES::Print( "Start");
long long int time = ARES::TimeStamp(true);
long long int bound =200;
long long int x =1;
long long int y =1;
while ( x< bound) {
++x;
y =1;
while ( y< bound) {
++y;
ARES::Print( ARES::Str( x, ":", y));
};
};
ARES::Print( ARES::Add( ARES::Add( "Exec took : ", ARES::TimeStamp(true) - time), "ms"));
ARES::Print( "002");
};
