#ifndef TILE
#define TILE
//Tile.h: 
//Created: Sat Sep 25 14:18:57 CDT 2021
#include <iostream>
#include <iomanip>
#include <map>
#include "TileColor.h"

class Tile {
    public:
        Tile(std::string strRep);
        bool isBoardTile;
        Tile* equivalentHandTile;
        friend std::ostream& operator<<(std::ostream& os,const Tile& tile);

        int getNumber() const;
        bool getIsJoker() const;
        TileColor getTileColor() const;
        void setSortableId(char id);
        void setJokerTileToValue(int number, TileColor color);

        std::string debugString();

        bool sameValue(const Tile& other);
        bool operator<(const Tile& other);
        static bool pointerLess(Tile* r,Tile* l);
        int toFastCalcTile();
        int getOriginalityNumber() const;

    private:
        int originalityNumber;
        int number;
        char id;
        bool isJoker;
        TileColor color;
        static std::map<std::string,int> dupCounter;

};
#endif//TILE
