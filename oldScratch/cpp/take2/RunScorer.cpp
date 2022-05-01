
#include "RunScorer.h"
#include <sstream>

RunCalcState::RunCalcState(){
    for(int i=0;i<4;i++){
        lastForColor[i]=INT_MAX;
        chainScore[i]=1;
        chainIsRunBreakingIfUnused[i]=false;
    }
}
std::string RunCalcState::debugPrint(){
    std::stringstream s;
    std::string output;
    s<<"["<<FastCalcTile::Printable(lastForColor[0]).toString();
    for(int i=1;i<4;i++){
        s<<","<<FastCalcTile::Printable(lastForColor[i]).toString();
    }
    s<<"] [";
    s<<(chainIsRunBreakingIfUnused[0]?'t':'f');
    for(int i=1;i<4;i++){
        s<<","<<(chainIsRunBreakingIfUnused[i]?'t':'f');
    }
    s<<"] [";
    s<<chainScore[0];
    for(int i=1;i<4;i++){
        s<<","<<chainScore[i];
    }
    s<<"]";
    std::getline(s,output);
    return output;
}
std::string RunScorer::debugTileIntListStr(int tiles[],unsigned int size){
    std::stringstream s;
    char colors[]={'B','R','T','Y'};
    s<<"[";
    if(size>0){
        s<<FastCalcTile::getNumber(tiles[0])<<colors[FastCalcTile::getTileColorIndex(tiles[0])];
        s<<(FastCalcTile::getIsBoardTile(tiles[0])?'b':'h');
        if(FastCalcTile::getHasEquivalentHandTile(tiles[0])) s<<"*";
        for(unsigned int i=1;i<size;i++){
            s<<",";
            s<<FastCalcTile::getNumber(tiles[i])<<colors[FastCalcTile::getTileColorIndex(tiles[i])];
            s<<(FastCalcTile::getIsBoardTile(tiles[i])?'b':'h');
            if(FastCalcTile::getHasEquivalentHandTile(tiles[i])) s<<"*";
        }
    }
    s<<"]";
    return s.str();
}
int RunScorer::score(const int sortedList1[],int sortedList1Size,const int sortedList2[], int sortedList2Size){
    int it1=0;
    int it2=0;
    int score=0;

    RunCalcState state;
    int current;

    while(it1<sortedList1Size&&it2<sortedList2Size){
        if(sortedList1[it1]<sortedList2[it2]){
            current=sortedList1[it1];
            if(updateBreaking(score,current,state)) return INT_MAX;
            it1++;
        }
        else if(sortedList2[it2]<sortedList1[it1]){
            current=sortedList2[it2];
            if(updateBreaking(score,current,state)) return INT_MAX;
            it2++;
        }
        //tile int encodes unique id, so never equal
    }
    while(it1<sortedList1Size){
        current=sortedList1[it1];
        if(updateBreaking(score,current,state)) return INT_MAX;
        it1++;
    }
    while(it2<sortedList2Size){
        current=sortedList2[it2];
        if(updateBreaking(score,current,state)) return INT_MAX;
        it2++;
    }
    for(int i=0;i<4;i++){
        if(state.chainScore[i]<3){
            if(state.chainIsRunBreakingIfUnused[i]){
                return INT_MAX;
            }

            score+=state.chainScore[i];
        }
    }
    return score;
}

    /*

 y
                      t                                b     y  r
 v                    v                                v     v  v
   1T 2T 3B 3R 4B 4R 4T 4Y 5B 5R 5Y 6B 6Y 9B 9R JB JY QB QY KY KR
      2        2  2        3  3  2  4   3        2     3  2  3
     */
bool RunScorer::updateBreaking(int& score,int current, RunCalcState& state){
    int colorNdx=FastCalcTile::getTileColorIndex(current);
    if(state.lastForColor[colorNdx]==INT_MAX){
        state.chainIsRunBreakingIfUnused[colorNdx]=FastCalcTile::getIsInvalidUnused(current);
    }
    else 
    {
        if(FastCalcTile::getNumber(current)==FastCalcTile::getNumber(state.lastForColor[colorNdx])+1){
            state.chainScore[colorNdx]++;
            state.chainIsRunBreakingIfUnused[colorNdx]|=FastCalcTile::getIsInvalidUnused(current);
            state.lastForColor[colorNdx]=current;
        }
        else {
            if(state.chainScore[colorNdx]<3){
                if(state.chainIsRunBreakingIfUnused[colorNdx]){
                    return true;
                }
                score+=state.chainScore[colorNdx];
            }
            state.chainScore[colorNdx]=1;
            state.chainIsRunBreakingIfUnused[colorNdx]=FastCalcTile::getIsInvalidUnused(current);
        }
    }
    state.lastForColor[colorNdx]=current;

    return false;
}
