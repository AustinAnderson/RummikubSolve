#include "TileColor.h"
std::string TileColorEnum::consoleCode(TileColor color){
    if(color==TileColor::BLACK) return "\u001b[0;30;47m";
    if(color==TileColor::RED) return "\u001b[0;31;47m";
    if(color==TileColor::TEAL) return "\u001b[1;34;47m";
    if(color==TileColor::YELLOW) return "\u001b[1;33;47m";
    std::string err="unhandled color '";
    err+=char(color);
    err+="'";
    throw err;
}
int TileColorEnum::ValuesIndex(TileColor color){
    if(color==TileColor::BLACK) return 0;
    if(color==TileColor::RED) return 1;
    if(color==TileColor::TEAL) return 2;
    if(color==TileColor::YELLOW) return 3;
    std::string err="unhandled color '";
    err+=char(color);
    err+="'";
    throw err;
}
const std::string TileColorEnum::ResetCode=  "\u001b[0m";
const std::string TileColorEnum::TileBG="\u001b[1;30;47m";
const std::array<TileColor,4> TileColorEnum::Values={{BLACK,RED,TEAL,YELLOW}};
