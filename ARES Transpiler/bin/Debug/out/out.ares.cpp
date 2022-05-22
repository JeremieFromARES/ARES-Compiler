// Using the ARES language 2022, under GPL3 license.
// Transpile date: 2022/50/21 04:50:09

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
long long int bound =8000;
long long int x =0;
long long int y =0;
long long int rdm;
ARES::Types::Array<long long int> rdm_array;
std::vector<long long int> rdm_vec;
while ( x< bound) {
x++;
y =0;
while ( y< bound) {
y++;
rdm = ARES::Round( ARES::Random(0,100),0);
rdm_array.Add( rdm);
};
};
ARES::Print( ARES::Str( "Execution took : \"", ARES::TimeStamp(true) -time, "\"ms"));
ARES::Print(rdm_array.Count());
};
