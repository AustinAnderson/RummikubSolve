#include "Tile.h"
#include "RunScorer.h"
#include "MaxGroup.h"
#include <iostream>
#include <sstream>
#include <vector>
#include <algorithm>
#include <limits.h>
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
            tile->isBordTile=true;
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
    //add the groups out of the above list that have a length >=3 to the final set,
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
    std::sort(groups->begin(),groups->end());
    std::sort(groupBaseUnused.begin(),groupBaseUnused.end(),Tile::pointerLess);
    return groups;
}


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
        if(allTiles.tiles[i]->sameValue((*allTiles.tiles[i+1]))){
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
    unsigned int currentDigit=0;
    bool done=false;
    //invalid should also get max int as score to make currentScore<score false
    int score=INT_MAX;
    std::vector<std::vector<GroupIteration*>::const_iterator> solution(groups->size());
    unsigned int totalPossibilities=1;
    for(unsigned int i=0;i<groups->size();i++){
        totalPossibilities*=(*groups)[i]->size();
    }
    std::vector<RunScorer*> scorers(totalPossibilities);

    int currentPossibility=0;
    RunScorer::allocateScoreData();
    while(!done){
        scorers[currentPossibility]=new RunScorer(groups,groupBaseUnused);
        /*
        int currentScore=scorers[currentPossibility]->score();
        if(currentScore<score){
            for(unsigned int i=0;i<groups->size();i++){
                solution[i]=(*groups)[i]->current();
            }
        }
        */
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
    for(unsigned int i=0;i<scorers.size();i++){
        delete scorers[i];
    }
    RunScorer::deallocateScoreData();
    for(unsigned int i=0;i<groups->size();i++){
        delete (*groups)[i];
    }
    delete groups;
    std::cout<<"done"<<std::endl;
    //solution something something something
    
}
