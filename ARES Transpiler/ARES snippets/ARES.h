#include <iostream>
#include <string.h>
#include <vector>
#include <sstream>
#include <cmath>
#include <random>
#include <chrono>

namespace ARES
{
    size_t random_counter = 0;

    /// Print( Var )
    template <typename Var> void
    Print(Var input) {
        std::cout << input << std::endl;
    }

    /// Input() >> Str
    std::string
    Input() {
        std::string in;
        std::getline(std::cin, in);
        return in;
    }

    /// Split( Str, Str ) >> Str[]
    std::vector<std::string>
    Split(std::string in_string, std::string delimiter) {
        size_t x;
        size_t y = 0;
        std::vector<std::string> v;
        while ((x = in_string.find_first_not_of(delimiter, y)) != std::string::npos) {
            y = in_string.find(delimiter, x);
            v.push_back(in_string.substr(x, y - x));
        }
        return v;
    }

    /// GetToken( Str, Str, Lng ) >> Str
    std::string
    GetToken(std::string in_string, std::string delimiter, size_t index) {
        std::vector<std::string> v = ARES::Split(in_string, delimiter);
        return v[index - 1];
    }

    /// Replace( Str, Str, Str ) >> Str
    std::string
    Replace(std::string in_string, std::string look_for, std::string replace_with) {
        size_t x;
        size_t y = 0;
        size_t u = look_for.size();
        size_t n = in_string.size();
        std::vector<std::string> z;
        while (1) {
            x = in_string.find(look_for, y);
            if (x == std::string::npos) { z.push_back(in_string.substr(y, n - y)); break;}
            z.push_back(in_string.substr(y, x - y) + replace_with);
            y = x + u;
        }
        std::string v = z[0];
        for (size_t i = 1; i < z.size(); i++) {
            v += z[i];
        }
        return v;
    }

    /// Find( Str, Str ) >> Lng
    size_t
    Find(std::string in_string, std::string to_find) {
        return in_string.find(to_find, 0);
    }

    /// FindCount( Str, Str ) >> Lng
    size_t
    FindCount(std::string in_string, std::string to_find) {
        size_t x;
        size_t y = 0;
        size_t u = to_find.size();
        size_t c = 0;
        while (1)
        {
            x = in_string.find(to_find, y);
            if (x == std::string::npos) { break;}
            ++c;
            y = x + u;
        }
        return c;
    }

    /// Append( Str, Str ) >> Str
    std::string
    Append(std::string in_string, std::string to_append) {
        return in_string.append(to_append);
    }
            std::string Append(std::string in_string, const char* to_append) { return in_string.append(to_append); }
            std::string Append(const char* in_string, std::string to_append) { return std::string(in_string).append(to_append); }
            std::string Append(const char* in_string, const char* to_append) { return std::string(in_string).append(to_append); }

    /// Append( Str, Dbl ) >> Str
    std::string
    Append(std::string in_string, long double to_append) {
        return in_string.append(std::to_string(to_append));
    }

    /// Append( Str, Var ) >> Str
    template <typename Var> std::string
    Append(std::string in_string, Var to_append) {
        return in_string.append(std::to_string(to_append));
    }

    /// Concat( Str, Str ) >> Str
    std::string
    Concat(std::string left, std::string right) {
        return ARES::Append(left, right);
    }

    /// SubStr( Str, Lng, Lng) >> Str
    std::string
    SubStr(std::string in_string, size_t offset, size_t length) {
        return in_string.substr(offset,length);
    }

    /// Left( Str, Lng ) Str
    std::string
    Left(std::string in_string, size_t length) {
        return ARES::SubStr(in_string, 0, length);
    }

    /// Right( Str, Lng ) >> Str
    std::string
    Right(std::string in_string, size_t length) {
        return ARES::SubStr(in_string, in_string.size() - length, length);
    }

    /// Length( Str ) >> Lng
    size_t
    Length(std::string in_string) {
        return in_string.size();
    }

    /// ToStr( Var ) >> Str
    template <typename Var> std::string
    ToStr(Var input) {
        std::stringstream v;
        v << input;
        return v.str();
    }

    /// Add( Dbl, Var ) >> Dbl
    template <typename Var> long double
    Add(long double left, Var right) {
        return left + right;
    }

    /// Add( Str, Var ) >> Str
    template <typename Var> std::string
    Add(std::string left, Var right) {
        return ARES::Append(left, right);
    }

    /// Substract( Dbl, Var ) >> Dbl
    long double
    Substract(long double left, long double right) {
        return left - right;
    }

    /// Multiply( Dbl, Var ) Dbl
    long double
    Multiply(long double left, long double right) {
        return left * right;
    }

    /// Divide( Dbl, Var ) >> Dbl
    long double
    Divide(long double left, long double right) {
        return left / right;
    }

    /// Pow( Dbl, Var ) >> Dbl
    long double
    Pow(long double left, long double right) {
        return std::pow(left, right);
    }

    /// Round( Dbl, Lng ) >> Dbl
    long double
    Round(long double number, unsigned int decimals) {
        int x = ARES::Pow(10, decimals);
        return round(number * x) / x;
    }

    /// Max( Dbl, Dbl ) >> Dbl
    long double
    Max(long double left, long double right) {
        if (left > right){ return left; } else { return right; };
    }

    /// Min( Dbl, Dbl ) >> Dbl
    long double
    Min(long double left, long double right) {
        if (left < right){ return left; } else { return right; };
    }

    /// Average( Dbl, Dbl ) >> Dbl
    long double
    Average(long double left, long double right) {
        return (left + right) / 2;
    }

    /// Random( Dbl, Dbl ) >> Dbl
    long double
    Random(long double min, long double max, size_t seed = 0) {
        ++random_counter;
        static std::uniform_real_distribution<> urd(min, max);
        static size_t rdm = std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch()).count();
        size_t rc = (rdm + random_counter) * (seed == 0) + seed;
        static std::default_random_engine dre(rc);
        return urd(dre);
    }

    /// Flip( Bol ) >> Bol
    bool
    Flip(bool boolean) {
        return !boolean;
    }
}