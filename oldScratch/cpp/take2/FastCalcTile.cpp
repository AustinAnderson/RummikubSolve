#include "FastCalcTile.h"
#include <iomanip>
std::string PrintableFastCalcTile::toString() const {
    std::stringstream os;
    os<<"{"<<std::setw(2)<<FastCalcTile::getNumber(value)<<(char)TileColorEnum::Values[FastCalcTile::getTileColorIndex(value)];
    os<<"(";
    if(FastCalcTile::getIsBoardTile(value)){
        os<<"B";
    }else{
        os<<"H";
    }
    if(FastCalcTile::getHasEquivalentHandTile(value)){
        os<<"*";
    }
    else{
        os<<" ";
    }
    os<<")_"<<std::setw(2)<<FastCalcTile::getId(value)<<"#"<<FastCalcTile::getOriginality(value)<<"}";
    return os.str();
}
std::ostream& operator<<(std::ostream& os,const PrintableFastCalcTile& tile)
{
    os<<tile.toString();
    return os;
}
//number should never be bigger than 15
int FastCalcTile::bundleFastCalcTile(int originality,int number, TileColor color,bool isBoardTile, bool hasEquivalentHandTile,short id){
    //         color
    //           |  isBoard
    //      num  |  | hasHandEquivalent
    //       |   |  | | padding       id
    //  _____|___|__|_|__/|            |
    // / originality| |   |            |
    // v v   v   v  v v   v            v
    // 0 00 0000 00 0 0 00000  0000000000000000
    // 0 00 0000 00 1 0 00000  0000000000000000 true
    // 0 00 0000 00 1 1 00000  0000000000000000 false
    // 0 00 0000 00 0 0 false
    //              0 1 false
    return (originality<<29) |(number<<25)| (TileColorEnum::ValuesIndex(color))<<23|isBoardTile<<22|hasEquivalentHandTile<<21|id;
}
bool FastCalcTile::getIsBoardTile(int tile){
    return (tile & IS_BOARD_TILE_MASK)==IS_BOARD_TILE_MASK;
}
bool FastCalcTile::getHasEquivalentHandTile(int tile){
    return (tile & HAS_EQUIVALENT_HAND_TILE)==HAS_EQUIVALENT_HAND_TILE;
}
bool FastCalcTile::getIsInvalidUnused(int tile){
             //if xx&11 ==10 then xx == 10, different from getIsBoardTile which will be true for 11 or 10
    return (tile & BOARD_WITH_EQUIVALENT_MASK) == IS_BOARD_TILE_MASK;
}
int FastCalcTile::getOriginality(int tile){
    return tile>>29;
}

int FastCalcTile::getTileColorIndex(int tile){
    return (COLOR_MASK & tile)>>23;
}
int FastCalcTile::getNumber(int tile){
    return ((unsigned)tile)>>25;
}
int FastCalcTile::getId(int tile){
    return (unsigned short)tile;
}
PrintableFastCalcTile FastCalcTile::Printable(int tile){
    return PrintableFastCalcTile{.value=tile};
}


