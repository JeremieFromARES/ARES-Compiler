#include <iostream>
#include <fstream>
#include <string.h>
#include <vector>
#include <sstream>
#include <cmath>
#include <random>
#include <chrono>
#include <algorithm>
#include <iomanip>
#include <time.h>

namespace ARES
{
    /// Private Globals
    /// Can only be accessed from methods inside of ARES.h
    namespace {
        bool initialized = false;
        size_t random_counter = 0;
        size_t random_default_seed = 0;
    }

    /// ARES type wrappers
    namespace Types {
        /// Array[]
        template<typename Var>
        struct Array {
            public:
                /// Private Value
                std::vector<Var> p_array;
                /// Default Constructor
                Array() {}
                /// Array Constructor from constant array.
                template <typename ... VarArgs>
                Array( VarArgs ... args) {
                    using expander = int[];
                    (void)expander{0,
                        (void( p_array.push_back(args) ), 0)...
                    };
                }
                /// Array = Var value
                void
                operator=(Var value[]) {
                    p_array = value;
                }
                void
                operator=(Array value) {
                    p_array = value.p_array;
                }
                /// Array == Array
                bool
                operator==(Var value[]) {
                    return value == p_array;
                }
                /// Array[Lng] >> Var
                Var
                operator[](size_t index) {
                    return p_array[index];
                }
                /// Array.Add(Var)
                void
                Add(Var value) {
                    p_array.push_back(value);
                }
                /// Array.Insert(Lng, Var)
                void
                Insert(Var value, size_t index) {
                    p_array.insert(p_array.begin() + index,value);
                }
                /// Array.Delete()
                void
                Delete() {
                    p_array.clear();
                }
                /// Array.Contains(Var) >> Bol
                bool
                Contains(Var value) {
                    auto n = std::find(p_array.begin(), p_array.end(), value);
                    long long int i = std::distance( p_array.begin(), n );
                    if (i == p_array.size())
                        return false;
                    else
                         return true;
                }
                /// Array.IndexOf(Var) >> Lng
                long long int
                IndexOf(Var value) {
                    auto n = std::find(p_array.begin(), p_array.end(), value);
                    long long int i = std::distance( p_array.begin(), n );
                    if (n == p_array.end())
                        return -1;
                    else
                         return i;
                }
                /// Array.RemoveIndex(Lng) >> Lng
                void
                RemoveIndex(long long int index) {
                    auto n = p_array.begin() + index;
                    p_array.erase(n);
                }
                /// Array.Count() >> Lng
                long long int
                Count() {
                    return p_array.size();
                }
        };
        struct String {
            public:
                /// value
                std::string p_string;
                /// Empty constructor
                String() {
                    p_string = std::string("");
                }
                /// std::string constructor
                String(std::string initializer) {
                    p_string = initializer;
                }
                /// const char* constructor
                String(const char* initializer) {
                    p_string = std::string(initializer);
                }
                /// Constructor
                String(String* initializer) {
                    p_string = initializer->p_string;
                }
                /// Str = Str
                void
                operator=(String value) {
                    p_string = value.p_string;
                }
                /// Str == Str
                bool
                operator==(String value) {
                    return value.p_string == p_string;
                }
                /// Str == Str
                bool
                operator==(std::string value) {
                    return value == p_string;
                }
                /// Str == Str
                bool
                operator==(const char* value) {
                    return value == p_string;
                }
                /// Str[Lng] >> Var
                String
                operator[](size_t index) {
                    std::string s("");
                    s.push_back(p_string[index]);
                    return String(s);
                }
                /// Str.Delete()
                void
                Delete() {
                    p_string.clear();
                }
                /// Str.Contains(Str) >> Bol
                bool
                Contains(String value) {
                    return p_string.find(value.p_string) != std::string::npos;
                }
                /// Str.Find(Str) >> Bol
                long long int
                Find(String value) {
                    size_t n = p_string.find(value.p_string);
                    if (n == std::string::npos)
                        return -1;
                    else
                        return n;
                }
                /// Str.Count() >> Lng
                long long int
                Length() {
                    return p_string.size();
                }
                /// Str.Split( Str ) >> Str[]
                Array<String>
                Split(String delimiter) {
                    size_t x;
                    size_t y = 0;
                    Array<String> v;
                    while ((x = p_string.find_first_not_of(delimiter.p_string, y)) != std::string::npos) {
                        y = p_string.find(delimiter.p_string, x);
                        v.Add(String(p_string.substr(x, y - x)));
                    }
                    return v;
                }
                /// Str.Replace( Str, Str ) >> Str
                String
                Replace(String look_for, String replace_with) {
                    std::string v;
                    size_t x;
                    size_t b;
                    size_t y = 0;
                    size_t u = look_for.p_string.size();
                    size_t n = p_string.size();
                    std::vector<std::string> z;
                    while (1) {
                        x = p_string.find(look_for.p_string, y);
                        if (x == std::string::npos) { z.push_back(p_string.substr(y, n - y)); break;}
                        z.push_back(p_string.substr(y, x - y) + replace_with.p_string);
                        y = x + u;
                    }
                    b = z.size();
                    v = z[0];
                    for (size_t i = 1; i < b; i++) {
                        v += z[i];
                    }
                    return String(v);
                }
        };
    }

    /// Private __ARES_MAIN_INIT__()
    /// Called at the start of main to initialize ARES dependencies.
    void
    __ARES_MAIN_INIT__() {
        if (initialized) {return;}
        initialized = true;
        random_default_seed = std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch()).count();
    }

    /// Print( Str )
    /// Writes to the console.
    void
    Print(ARES::Types::String input) {
        std::cout << std::string(input.p_string).append("\n");
    }
            void Print(std::string input) { std::cout << std::string(input).append("\n"); }
            void Print(const char* input) { std::cout << std::string(input).append("\n"); }
            void Print(bool input) { if (input) {std::cout << std::string("true").append("\n");} else {std::cout << std::string("false").append("\n");} }

    /// Print( Var )
    /// Writes to the console.
    template <typename Var> void
    Print(Var input) {
        std::cout << std::to_string(input).append("\n");
    }

    /// Input() >> Str
    /// Holds the execution of the program until the user presses [Enter]. Any text input before will be returned as a String.
    ARES::Types::String
    Input() {
        std::string in;
        std::getline(std::cin, in);
        return in;
    }

    /// Split( Str, Str ) >> Str[]
    /// Returns an Array of Strings by splitting a String by a delimiter.
    ARES::Types::Array<ARES::Types::String>
    Split(ARES::Types::String in_string, ARES::Types::String delimiter) {
        size_t x;
        size_t y = 0;
        ARES::Types::Array<ARES::Types::String> v;
        while ((x = in_string.p_string.find_first_not_of(delimiter.p_string, y)) != std::string::npos) {
            y = in_string.p_string.find(delimiter.p_string, x);
            v.p_array.push_back(in_string.p_string.substr(x, y - x));
        }
        return v;
    }

    /// Replace( Str in_string, Str look_for, Str replace_with ) >> Str
    /// Replaces a portion of a String by another.
    ARES::Types::String
    Replace(ARES::Types::String in_string, ARES::Types::String look_for, ARES::Types::String replace_with) {
        std::vector<std::string> z;
        std::string v;
        size_t x, b, y, u, n;
        y = 0;
        u = look_for.p_string.size();
        n = in_string.p_string.size();
        while (1) {
            x = in_string.p_string.find(look_for.p_string, y);
            if (x == std::string::npos) { z.push_back(in_string.p_string.substr(y, n - y)); break;}
            z.push_back(in_string.p_string.substr(y, x - y) + replace_with.p_string);
            y = x + u;
        }
        b = z.size();
        v = z[0];
        for (size_t i = 1; i < b; i++) {
            v += z[i];
        }
        return v;
    }

    /// Find( Str, Str ) >> Lng
    /// Returns the position of the first match of a String inside another String.
    long long int
    Find(ARES::Types::String in_string, ARES::Types::String to_find) {
        long long int pos = in_string.p_string.find(to_find.p_string, 0);
        if (pos == std::string::npos) {
            pos = -1;
        }
        return pos;
    }

    /// Str.Contains( Str ) >> Bol
    /// Returns true if the String contains another String. 
    bool
    Contains(ARES::Types::String in_string, ARES::Types::String to_find) {
        return in_string.p_string.find(to_find.p_string) != std::string::npos;
    }

    /// FindCount( Str, Str ) >> Lng
    /// Returns the number of matches inside a String.
    long long int
    FindCount(ARES::Types::String in_string, ARES::Types::String to_find) {
        size_t x, y, u, c;
        y = 0;
        u = to_find.p_string.size();
        c = 0;
        while (1)
        {
            x = in_string.p_string.find(to_find.p_string, y);
            if (x == std::string::npos) { break;}
            ++c;
            y = x + u;
        }
        return c;
    }

    /// Append( Str, Str ) >> Str
    /// Adds a String at the end of another String.
    ARES::Types::String
    Append(ARES::Types::String in_string, ARES::Types::String to_append) {
        return in_string.p_string.append(to_append.p_string);
    }
            ARES::Types::String Append(ARES::Types::String in_string, const char* to_append) { return in_string.p_string.append(to_append); }
            ARES::Types::String Append(const char* in_string, ARES::Types::String to_append) { return std::string(in_string).append(to_append.p_string); }
            ARES::Types::String Append(const char* in_string, const char* to_append) { return std::string(in_string).append(to_append); }
            ARES::Types::String Append(ARES::Types::String in_string, bool to_append) { if (to_append) { return std::string(in_string.p_string).append("true"); } else { return std::string(in_string.p_string).append("false"); } }

    /// Append( Str, Dbl ) >> Str
    /// Adds a String at the end of another String.
    ARES::Types::String
    Append(ARES::Types::String in_string, long double to_append) {
        return in_string.p_string.append(std::to_string(to_append));
    }

    /// Append( Str, Var ) >> Str
    /// Adds a String at the end of another String.
    template <typename Var> ARES::Types::String
    Append(ARES::Types::String in_string, Var to_append) {
        return in_string.p_string.append(std::to_string(to_append));
    }

    /// SubStr( Str, Lng, Lng ) >> Str
    /// Returns a new String containing n characters from a position in a String.
    ARES::Types::String
    SubStr(ARES::Types::String in_string, size_t offset, size_t length) {
        return in_string.p_string.substr(offset,length);
    }

    /// Left( Str, Lng ) Str
    /// Returns a new String containing n characters from the left of a String.
    ARES::Types::String
    Left(ARES::Types::String in_string, size_t length) {
        return ARES::SubStr(in_string, 0, length);
    }

    /// Right( Str, Lng ) >> Str
    /// Returns a new String containing n characters from the right of a String.
    ARES::Types::String
    Right(ARES::Types::String in_string, size_t length) {
        return ARES::SubStr(in_string, in_string.p_string.size() - length, length);
    }

    /// Length( Str ) >> Lng
    /// Returns the number of charcters in a String.
    long long int
    Length(ARES::Types::String in_string) {
        return in_string.p_string.size();
    }

    /// ToStr( Var ) >> Str
    /// Converts an argument to String.
    template <typename Var> ARES::Types::String
    ToStr(Var input) {
        return std::to_string(input);
    }
            ARES::Types::String ToStr(ARES::Types::String input) { return input; }
            ARES::Types::String ToStr(std::string input) { return input; }
            ARES::Types::String ToStr(const char* input) { return input; }
            ARES::Types::String ToStr(bool input) { if (input) {return std::string("true");} else {return std::string("false");} }
    
    /// Str( Var, ...) >> Str
    /// Converts all arguments to String and appends them to a single String.
    template <typename ... Var> ARES::Types::String
    Str( Var ... args) {
        std::string s;
        using expander = int[];
        (void)expander{0,
            (void( s.append(ARES::ToStr(args).p_string) ), 0)...
        };
        return s;
    }

    /// Add( Dbl, Dbl ) >> Dbl
    /// Substracts a Double to another.
    long double
    Add(long double left, long double right) {
        return left + right;
    }

    /// Substract( Dbl, Var ) >> Dbl
    /// Substracts a Double to another.
    long double
    Substract(long double left, long double right) {
        return left - right;
    }

    /// Multiply( Dbl, Dbl ) Dbl
    /// Multiplies a Double by another.
    long double
    Multiply(long double left, long double right) {
        return left * right;
    }

    /// Divide( Dbl, Dbl ) >> Dbl
    /// Divides a Double by another.
    long double
    Divide(long double left, long double right) {
        return left / right;
    }

    /// Pow( Dbl, Dbl ) >> Dbl
    /// Raises a Double to the power of n.
    long double
    Pow(long double left, long double right) {
        return std::pow(left, right);
    }

    /// Round( Dbl, Lng ) >> Dbl
    /// Returns a Double rounded to n decimals.
    long double
    Round(long double number, unsigned int decimals = 0) {
        int x = std::pow(10, decimals);
        return round(number * x) / x;
    }

    /// Max( Dbl, Dbl ) >> Dbl
    /// Returns the biggest of two numbers.
    long double
    Max(long double left, long double right) {
        if (left > right) { return left; } else { return right; };
    }

    /// Min( Dbl, Dbl ) >> Dbl
    /// Returns the smallest of two numbers.
    long double
    Min(long double left, long double right) {
        if (left < right){ return left; } else { return right; };
    }

    /// Average( Dbl, Dbl ) >> Dbl
    /// Returns the average of two numbers.
    long double
    Average(long double left, long double right) {
        return (left + right) / 2;
    }

    /// Random( Dbl, Dbl, Lng ) >> Dbl
    /// Returns a random Double between two numbers. A custom seed can be provided.
    long double
    Random(long double min = 0, long double max = 1, size_t seed = 0) {
        ++random_counter;
        static std::uniform_real_distribution<> urd(min, max);
        size_t rc = (random_default_seed + random_counter) * (seed == 0) + seed;
        static std::default_random_engine dre(rc);
        return urd(dre);
    }

    /// RandomBol( Lng ) >> Bol
    /// Returns a random boolean.
    bool
    RandomBol(size_t seed = 0) {
        ++random_counter;
        static std::uniform_real_distribution<> urd(0, 1);
        size_t rc = (random_default_seed + random_counter) * (seed == 0) + seed;
        static std::default_random_engine dre(rc);
        return round(urd(dre));
    }

    /// Flip( Bol ) >> Bol
    /// Returns the inverse of an inputed boolean.
    bool
    Flip(bool boolean) {
        return !boolean;
    }

    namespace Time {

        /// Time.Now() >> Lng
        /// Returns current date/time in milliseconds as a timestamp.
        long long int
        Now() {
            return std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch()).count();
        }

        /// Time.Date() >> Lng
        /// Creates a timestamp based on the inputed String (Format: YYYY-MM-DD HH:MM:SS).
        long long int
        Date(ARES::Types::String date) {
            std::tm t = {};
            std::stringstream s(date.p_string);
            s >> std::get_time(&t, "%Y-%m-%d %H:%M:%S");
            auto y = std::chrono::system_clock::from_time_t(_mkgmtime(&t));
            return std::chrono::duration_cast<std::chrono::milliseconds>(y.time_since_epoch()).count();
        }

        /// Time.Date() >> Lng
        /// Creates a timestamp based on the inputed date.
        long long int
        Date(long long int year, long long int month, long long int day, long long int hour = 0, long long int minute = 0, long long int second = 0) {
            std::stringstream s;
            s << std::setw(4) << std::setfill('0') << year;
            s << "-";
            s << std::setw(2) << std::setfill('0') << month;
            s << "-";
            s << std::setw(2) << std::setfill('0') << day;
            s << " ";
            s << std::setw(2) << std::setfill('0') << hour;
            s << ":";
            s << std::setw(2) << std::setfill('0') << minute;
            s << ":";
            s << std::setw(2) << std::setfill('0') << second;
            std::setfill(' ');
            std::tm t = {};
            s >> std::get_time(&t, "%Y-%m-%d %H:%M:%S");
            return ((long long int) _mkgmtime(&t)) * 1000;
        }

        /// Time.DateToStr() >> Str
        /// Takes a timestamp(milliseconds) and returns a String of its human-readable form.
        ARES::Types::String
        DateToStr(long long int date) {
            time_t t = (time_t)(date / 1000ll);
            std::tm m = {};
            char buf[32] = { 0 };
            std::strftime(buf, sizeof(buf), "%Y-%m-%d %H:%M:%S", gmtime(&t));
            return std::string(buf);
        }
    }

    namespace Debug {

        /// Debug.ExecTime() >> Lng
        /// Returns the number of milliseconds ellapsed since program initialization.
        long long int
        ExecTime() {
            return std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch()).count() - random_default_seed;
        }

        /// Debug.Assert( Bol )
        /// Aborts the program if condition is false.
        void
        Assert(bool condition) {
            if (condition == false) {abort();}
        }
    }

    namespace Sys
    {

    }

    namespace App
    {

    }
}