#include "Tile.h"
#include "RunScorer.h"
#include "MaxGroup.h"
#include <iostream>
#include <sstream>
#include <vector>
#include <algorithm>
#include <limits.h>
#include "DebugUtil.h"
struct TileSet
{
    TileSet(){}
    TileSet(const TileSet& other)=delete;
    ~TileSet(){
        for(unsigned int i=0;i<tiles.size();i++){
            delete tiles[i];
        }
    }
    std::vector<Tile*> tiles;
};
struct DisplayRunOrGroup{
    DisplayRunOrGroup(std::string runOrGroup, TileSet& allTiles){
        std::stringstream ss(runOrGroup);
        std::string tileRep;
        while(std::getline(ss,tileRep,',')){
            Tile* tile=new Tile(tileRep);
            tile->isBoardTile=true;
            tiles.push_back(tile);
            allTiles.tiles.push_back(tile);
        }
    }
    std::vector<Tile*> tiles;
    friend std::ostream& operator<<(std::ostream& os, const DisplayRunOrGroup& data){
        os<<(*(data.tiles[0]));
        for(unsigned int i=1;i<data.tiles.size();i++){
            os<<" "<<(*(data.tiles[i]));
        }
        return os;
    }
};
struct DisplayState{
    DisplayState(){}
    DisplayState(std::string board,std::string hand,TileSet& allTiles){
        std::stringstream boardParser(board);
        std::string runOrGroupRep;
        while(std::getline(boardParser,runOrGroupRep,' ')){
            runsAndGroups.push_back(DisplayRunOrGroup(runOrGroupRep,allTiles));
        }
        std::stringstream handParser(hand);
        std::string tileRep;
        while(std::getline(handParser,tileRep,',')){
            Tile* tile=new Tile(tileRep);
            this->hand.push_back(tile);
            allTiles.tiles.push_back(tile);
        }
    }
    std::vector<DisplayRunOrGroup> runsAndGroups;
    std::vector<Tile*> hand;
    friend std::ostream& operator<<(std::ostream& os,const DisplayState& data){
        for(unsigned int i=0;i<data.runsAndGroups.size();i++){
            os<<data.runsAndGroups[i]<<std::endl;
        }
        os<<"____________________________________________________________________________________________"<<std::endl;

        if(data.hand.size()>0){
            os<< (*(data.hand[0]));
            for(unsigned int i=1;i<data.hand.size();i++){
                os<<" "<<(*(data.hand[i]));
            }
            os<<std::endl;
        }
        return os;
    }
};
std::ostream& operator<<(std::ostream& os,const std::vector<Tile*>& tiles)
{
    if(tiles.size()>0){
        os<<(*tiles[0]);
        for(unsigned int i=1;i<tiles.size();i++){
            os<<" "<<(*tiles[i]);
        }
    }
    return os;
}


//groupBaseUnusedShould be empty
std::vector<MaxGroup*>* findMaxGroups(const TileSet& allTiles,std::vector<Tile*>& groupBaseUnused){
    std::vector<MaxGroup*>* groups=new std::vector<MaxGroup*>();
    //pull out all the dups and put them on the end
    //then group adjacent same numbered tiles, allowing the dups to fall into their own group
    //this presents an edge case when there are 2 dups, where it should be split into 2 3-groups,
    //but will be split into 1 4-group and 2 unused, so handle that manually
    //BRTYBR -> BRTY  BR, need to move the Y over

    //this kinda thing would normally be a linked list type use case,
    //but will only run at most 26*26 times over a list that is at most 106 long
    //so not worth the hassle of converting between list types
    std::vector<Tile*> dups;
    groupBaseUnused.clear();

    for(unsigned int i=0;i<allTiles.tiles.size();i++){
        groupBaseUnused.push_back(allTiles.tiles[i]);
    }
    for(auto it=groupBaseUnused.end()-1;it!=groupBaseUnused.begin();it--)
    {
        if((*it)->sameValue(*(*(it-1)))){
            dups.insert(dups.begin(),*it);
            groupBaseUnused.erase(it);
        }
    }
    for(unsigned int i=0;i<dups.size();i++){
        groupBaseUnused.push_back(dups[i]);
    }
    //groupBaseUnused now has the sorted lists laid end to end
    //group consecutive same numbered tiles each into it's own list
    //1,1,2,2,4,5,6,7,7,7 becomes
    //{1,1},{2,2},{4},{5},{6},{7,7,7}
    std::vector<std::vector<Tile*>> potentialGroups;
    bool newAdd=true;
    for(unsigned int i=0;i<groupBaseUnused.size()-1;i++)
    {
        if(newAdd){
            std::vector<Tile*> current;
            current.push_back(groupBaseUnused[i]);
            potentialGroups.push_back(current);
        }
        if(groupBaseUnused[i]->getNumber()==groupBaseUnused[i+1]->getNumber())
        {
            potentialGroups[potentialGroups.size()-1].push_back(groupBaseUnused[i+1]);
            newAdd=false;
        }else{
            newAdd=true;
        }
    }
    //handle the edge case of a group of 4 with 2 duplicates
    std::sort(
            potentialGroups.begin(),potentialGroups.end(),
            //sort by group number ascending, then by size descending
            [&](std::vector<Tile*> v1,std::vector<Tile*> v2){
                int comp= v1[0]->getNumber()-v2[0]->getNumber();
                if(comp==0) comp=v2.size()-v1.size();
                return comp<0;
            }
    );
    for(unsigned int i=0;i<potentialGroups.size()-1;i++){
        if(potentialGroups[i].size()==4 && potentialGroups[i+1].size()==2 && potentialGroups[i][0]->getNumber()==potentialGroups[i+1][0]->getNumber()){
            potentialGroups[i].push_back(potentialGroups[i+1][0]);
            potentialGroups[i].push_back(potentialGroups[i+1][1]);
        }
    }
    //with the groups of 4 that have 2 dups converted to groups of 6,
    //add the groups out of the above list that have a length >=3 to the final set,
    //the "groups of 6" should be handled by MaxGroup constructor
    for(unsigned int i=0;i<potentialGroups.size();i++)
    {
        if(potentialGroups[i].size()>=3){
            groups->push_back(new MaxGroup(potentialGroups[i]));
            //if one of the potential groups got chosen, all of it's tiles have to be removed from the unused list
            for(unsigned int j=0;j<potentialGroups[i].size();j++)
            {
                //find the tile to be removed
                std::vector<Tile*>::iterator it=groupBaseUnused.begin();
                while(it!=groupBaseUnused.end()&& *it!=potentialGroups[i][j]){
                    it++;
                }
                if(it==groupBaseUnused.end()){
                    throw "cant remove tile from unused when finding max groups";
                }
                //remove it
                groupBaseUnused.erase(it);
            }
        }
    }
    return groups;
}


ENSURE_DEBUGUTIL_AVAILABLE(Tile);
ENSURE_DEBUGUTIL_AVAILABLE(MaxGroup);
int main(){
    TileSet allTiles;
    DisplayState test(
            std::string("6Y,7Y,8Y,9Y,10Y 2B,3B,4B,5B,6B ")+
            std::string("12B,12T,12R  11B,11Y,11R 10B,10T,10Y ")+
            std::string("12Y,12T,12R  2R,3R,4R 8B,9B,10B   4B,4T,4Y,4R   1B,1T,1Y   11T,11B,11Y ")+
            std::string("5R,5Y,5T,5B"),
        "2Y,7B,9B,1T,4T,3Y,5Y,12Y,1R,3R,9R,9R,8R,7T,3B,4Y",
        allTiles
    );
    std::sort(allTiles.tiles.begin(),allTiles.tiles.end(),Tile::pointerLess);
    for(unsigned char i=0;i<allTiles.tiles.size()-1;i++){
        allTiles.tiles[i]->setSortableId(i);
        if(allTiles.tiles[i]->sameValue((*allTiles.tiles[i+1]))&&allTiles.tiles[i]->isBoardTile&&!allTiles.tiles[i+1]->isBoardTile){
            //sorted by number,color,!isBoard; that means if two next to each other
            //first is board and second is hand
            allTiles.tiles[i]->equivalentHandTile=allTiles.tiles[i+1];
        }
    }
    allTiles.tiles[allTiles.tiles.size()-1]->setSortableId(allTiles.tiles.size()-1);

    //allTiles shouldn't be changed past this point!!!
    std::vector<Tile*> groupBaseUnused;
    std::vector<MaxGroup*>* groups=findMaxGroups(allTiles,groupBaseUnused);



    //iterate over all the max groups combinations, for each one get the score leftover after finding runs
    //model it after counting, carrying the one by incrementing the next group
    //and reseting all the ones before it
    //
    //
    //TODO:potential optimization by using pointer arithmetic and the same array for base unused and added unused
    int* groupBaseUnusedFastCalc=new int[groupBaseUnused.size()];
    for(unsigned int i=0;i<groupBaseUnused.size();i++)
    {
        groupBaseUnusedFastCalc[i]=groupBaseUnused[i]->toFastCalcTile();
    }
    //sort by fastcalctile int, which will handle dups
    std::sort(groupBaseUnusedFastCalc,groupBaseUnusedFastCalc+groupBaseUnused.size());
    std::sort(groups->begin(),groups->end(),MaxGroup::pointerLess);
    //sort groupBaseUnused and MaxGroups
    //debug
    int expectedPossibilitiesCount=1;
    for(unsigned int i=0;i<groups->size();i++){
        expectedPossibilitiesCount*=(*groups)[i]->getPossibilityCount();
        std::cout<<(*groups)[i]->debugString();
    }
    std::cout<<std::endl;
    std::cout<<"checking "<<expectedPossibilitiesCount<<" possibilities..."<<std::endl;
    //debug
    
    int* currentPossibilitySetFastCalc=new int[groups->size()*6];

    unsigned int currentDigit=0;
    bool done=false;
    //invalid should also get max int as score to make currentScore<score false
    int score=INT_MAX;
    std::vector<unsigned int> solutionKey(groups->size());
    std::stringstream currentConfigStr;
    unsigned long currentConfig=0;
    int currentPossibility=0;
    while(!done){
        int possibilitySetSize=0;
        currentConfig=0;
        currentConfigStr.str("");
        currentConfigStr.clear();
        for(unsigned int i=0;i<groups->size();i++){
            //possibilitySetSize by ref, increments to always be the last element
            (*groups)[i]->addCurrentUnused(currentPossibilitySetFastCalc,possibilitySetSize);
            currentConfigStr<<((*groups)[i]->getCurrentPossibilityKey());
        }
        currentConfigStr>>currentConfig;
        if(currentConfig==2005050001640){
            std::cout<<"yay"<<std::endl;//GDB
        }
        int currentScore=RunScorer::score(groupBaseUnusedFastCalc,groupBaseUnused.size(),currentPossibilitySetFastCalc,possibilitySetSize);
        if(currentScore<score){
            for(unsigned int i=0;i<groups->size();i++){
                solutionKey[i]=(*groups)[i]->getCurrentPossibilityKey();
            }
        }
        if((*groups)[currentDigit]->isAtLast())
        {
            while(!done&&(*groups)[currentDigit]->isAtLast()){
                currentDigit++;
                if(currentDigit>=groups->size()) 
                {
                    done=true;
                }
            }
            if(!done){
                (*groups)[currentDigit]->moveNext();
                for(unsigned int i=0;i<currentDigit;i++){
                    (*groups)[i]->resetIteration();
                }
                currentDigit=0;
            }
        }
        else{
            (*groups)[currentDigit]->moveNext();
        }
        currentPossibility++;
        if(currentPossibility%100000==0){
            std::cout<<currentPossibility<<" possibilities visited"<<std::endl;
        }
    }

    delete[] groupBaseUnusedFastCalc;
    delete[] currentPossibilitySetFastCalc;
    std::cout<<"done"<<std::endl;
    if(score<INT_MAX){
        std::cout<<"solution found at"<<std::endl;
        std::cout<<"[";
        if(solutionKey.size()>0){
            std::cout<<solutionKey[0];
            for(unsigned int i=1;i<solutionKey.size();i++){
                std::cout<<","<<solutionKey[i];
            }
        }
        std::cout<<"]"<<std::endl;
    }
    else{
        std::cout<<"no solution found"<<std::endl;
    }
    //solution something something something
    for(unsigned int i=0;i<groups->size();i++){
        delete (*groups)[i];
    }
    delete groups;
    
}
