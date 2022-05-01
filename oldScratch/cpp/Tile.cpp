/*
Tile.cpp: ...
Created: Sat Sep 25 14:20:21 CDT 2021
*/
#include "Tile.h"
Tile::Tile(std::string strRep){
    if(strRep=="J"){
        isJoker=true;
        number=0;
        color=TileColor::BLACK;
    }else{
        isJoker=false;
        number=std::atoi(strRep.substr(0,2).c_str());
        color=(TileColor)strRep[strRep.length()-1];
    }
    
}
std::ostream& operator<<(std::ostream& os,const Tile& tile)
{
    os<<TileColorEnum::TileBG<<"("<<std::setw(2)<<std::setfill('0')<<std::hex<<")|";
    if(tile.isJoker){
        os<<" J";
    }else{
        os<<TileColorEnum::consoleCode(tile.color)<<std::setw(2)<<std::setfill(' ')<<tile.number;
    }
    os<<TileColorEnum::TileBG<<"|"<<TileColorEnum::ResetCode;
    return os;
}

int Tile::getNumber() const {return number;}
bool Tile::getIsJoker() const {return isJoker;}
TileColor Tile::getTileColor() const {return color;}
void Tile::setSortableId(char id){
    this->id=id;
}
void Tile::setJokerTileToValue(int number, TileColor color){
    this->number=number;
    this->color=color;
}
bool Tile::sameValue(const Tile& other){
    return number==other.number && color==other.color;
}
bool Tile::operator<(const Tile& other){
    int comp=number-other.number;
    if(comp==0){
        comp=color-other.color;
    }
    if(comp==0){
        comp=(!isBordTile)-(!other.isBordTile);
    }
    return comp<0;
}
bool Tile::pointerLess(Tile* r,Tile* l){
    return *r<*l;
}
