/* RunScorer.cpp
   Created: Mon Sep 27 22:25:19 CDT 2021
*/

#include "RunScorer.h"

RunScorer::RunScorer(const std::vector<MaxGroup*>* groups,const std::vector<Tile*>& groupBaseUnused){
    MaxGroupListIterator groupFlatmapIterator(groups);
    sortedUnusedLength=groupFlatmapIterator.size()+groupBaseUnused.size();
    sortedUnused=new Tile*[sortedUnusedLength];
    int sortedUnusedNdx=0;
    bool groupsDone=groupFlatmapIterator.size()==0;
    auto baseUnusedIt=groupBaseUnused.begin();
    while(!groupsDone&&baseUnusedIt!=groupBaseUnused.end())
    {
        if(groupFlatmapIterator.current()->getNumber() < (*baseUnusedIt)->getNumber())
        {
            sortedUnused[sortedUnusedNdx]=groupFlatmapIterator.current();
            sortedUnusedNdx++;
            groupsDone=!groupFlatmapIterator.moveNext();
        }
        else if((*baseUnusedIt)->getNumber() < groupFlatmapIterator.current()->getNumber())
        {
            sortedUnused[sortedUnusedNdx]=*baseUnusedIt;
            baseUnusedIt++;
            sortedUnusedNdx++;
        }
        else//==
        {
            sortedUnused[sortedUnusedNdx]=groupFlatmapIterator.current();
            sortedUnusedNdx++;
            groupsDone=!groupFlatmapIterator.moveNext();


            sortedUnused[sortedUnusedNdx]=*baseUnusedIt;
            baseUnusedIt++;
            sortedUnusedNdx++;
        }
               
    }

    while(!groupsDone)
    {
        sortedUnused[sortedUnusedNdx]=groupFlatmapIterator.current();
        sortedUnusedNdx++;
        groupsDone=!groupFlatmapIterator.moveNext();
    }
    while(baseUnusedIt!=groupBaseUnused.end())
    {
        sortedUnused[sortedUnusedNdx]=*baseUnusedIt;
        baseUnusedIt++;
        sortedUnusedNdx++;
    }

}
RunScorer::~RunScorer(){
    delete[] sortedUnused;
}
int RunScorer::score()
{
    if(ScoreDataMemSize<=sortedUnusedLength){
        std::stringstream errbuilder;
        std::string err;
        errbuilder<<"ScoreDataMemSize (106)<=sortedUnusedLength ("<<sortedUnusedLength<<")";
        errbuilder>>err;
        throw err;
    }
    for(unsigned int i=0;i<scoreData.size();i++){
        std::fill(scoreData[i],scoreData[i]+ScoreDataMemSize,0);
    }
    currentCount.fill(1);
    lastNumber.fill(-1);
    for(unsigned int i=0;i<sortedUnusedLength;i++){
        int colorndx=getColorIndex(sortedUnused[i]);
        if(sortedUnused[i]->getNumber()==lastNumber[colorndx]+1){
            currentCount[colorndx]++;
        }
        else{
            currentCount[colorndx]=1;
        }
        lastNumber[colorndx]=sortedUnused[i]->getNumber();
        scoreData[colorndx][i]=currentCount[colorndx];

    }
    int score=0;
    lastNumber.fill(0);
    lastLastNumber.fill(0);
    for(int i=sortedUnusedLength-1;i>=0;i--)
    {
        int colorndx= getColorIndex(sortedUnused[i]);
        if(
            //if we get to a 1 and the last two non zeroed are not 2 and 3,
            //then this tile is not part of a group, or part of a 2 group
            (scoreData[colorndx][i]==1 && !(lastNumber[colorndx]==2 && lastLastNumber[colorndx]==3))
            ||
            //if we get to a 2 and the last non zeroed was not 3,
            //then this tile is part group of 2, which isn't valid so its unused
            (scoreData[colorndx][i]==2 && lastNumber[colorndx]!=3)
        ){

            if(sortedUnused[i]->isBordTile && sortedUnused[i]->equivalentHandTile==nullptr){
                //invalid solution, done immediately
                return INT_MAX;
            }
            else{
                score++;
            }
        }

        if(scoreData[colorndx][i]!=0){
            lastLastNumber[colorndx]=lastNumber[colorndx];
            lastNumber[colorndx]=scoreData[colorndx][i];
        }
    }

    return score;
}
//static
void RunScorer::allocateScoreData(){
    scoreData[0]=new int[106];
    scoreData[1]=new int[106];
    scoreData[2]=new int[106];
    scoreData[3]=new int[106];
}
//static
void RunScorer::deallocateScoreData(){
    delete[] scoreData[0];
    delete[] scoreData[1];
    delete[] scoreData[2];
    delete[] scoreData[3];
}
//private
int RunScorer::getColorIndex(const Tile* tile){
    switch(tile->getTileColor()){
        case TileColor::RED: return 0;
        case TileColor::BLACK: return 1;
        case TileColor::TEAL: return 2;
        case TileColor::YELLOW: return 3;
    }
}
std::array<int,4> RunScorer::currentCount=std::array<int,4>();
std::array<int,4> RunScorer::lastNumber=std::array<int,4>();
std::array<int,4> RunScorer::lastLastNumber=std::array<int,4>();
std::array<int*,4> RunScorer::scoreData=std::array<int*,4>();
