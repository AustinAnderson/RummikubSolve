#ifndef TILECOLOR
#define TILECOLOR
#include<array>
//TileColor.h: 
//Created: Sat Sep 25 14:20:12 CDT 2021

enum TileColor:char {
    BLACK='B',RED='R',TEAL='T',YELLOW='Y'
};
struct TileColorEnum{
    static const std::array<TileColor,4> Values;
    static int ValuesIndex(TileColor color);
    static std::string consoleCode(TileColor color);
    static const std::string ResetCode;
    static const std::string TileBG;
};
#endif//TILECOLOR
