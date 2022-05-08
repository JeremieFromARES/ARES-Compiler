// Using the ARES language 2022, under GPL3 license.
// Transpile date: 2022/55/08 09:55:01

// ARES - Headers
#include "ARES.h"

// ARES - Overhead prototypes
int main();

// ARES - Objects

// ARES - Program
int main() {
ARES::Init();
long long int time =TimeStamp();
long int i =0;
long long int bound =50000;
while ( i< bound) {
ARES::Print( i);
i = i +1;
};
ARES::Print(TimeStamp() - time);
};
