#ifndef FASTCALCTILE
#define FASTCALCTILE
//FastCalcTile.h: 
//Created: Thu Sep 30 20:38:57 CDT 2021
#include "TileColor.h"
#include <iostream>
#include <sstream>
struct PrintableFastCalcTile{
    int value;
    std::string toString() const;
    friend std::ostream& operator<<(std::ostream& os,const PrintableFastCalcTile& tile);
};

struct FastCalcTile {
    //number should never be bigger than 15
    static int bundleFastCalcTile(int originality,int number, TileColor color,bool isBoardTile, bool hasEquivalentHandTile,short id);

    static constexpr int IS_BOARD_TILE_MASK=2<<21;
    static bool getIsBoardTile(int tile);

    static constexpr int HAS_EQUIVALENT_HAND_TILE=1<<21;
    static bool getHasEquivalentHandTile(int tile);

    static constexpr int BOARD_WITH_EQUIVALENT_MASK=IS_BOARD_TILE_MASK|HAS_EQUIVALENT_HAND_TILE;
    static bool getIsInvalidUnused(int tile);

    static constexpr int COLOR_MASK=3<<23;
    static int getTileColorIndex(int tile);

    static int getOriginality(int tile);

    static int getNumber(int tile);

    static int getId(int tile);

    static PrintableFastCalcTile Printable(int tile);
};
#endif//FASTCALCTILE
