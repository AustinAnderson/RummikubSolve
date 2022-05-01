#include "MaxGroup.h"
#include "FastCalcTile.h"
//max groups creation is not performance critical, so check constraints
MaxGroup::MaxGroup(const std::vector<Tile*>& tilesFound){
    for(unsigned int i=0;i<tilesFound.size();i++){
        allGroup.push_back(tilesFound[i]->toFastCalcTile());
    }
    selected=0;

    //if 3, all or nothing,
    //if 4, all, nothing, and the 4 3-groups leaving out 1
    //if 6, same as 4 but also the possibility of 2 3-groups at the same time
    possibilityCount=7;
    if(allGroup.size()==3) {
        possibilityCount=2;
    }
    else if(allGroup.size()==4){
        possibilityCount=6;
    }
    //validate
    std::stringstream err;
    err<<"invalid group [";
    if(tilesFound.size()>0){
        err<<tilesFound[0]->debugString();
        for(unsigned int i=0;i<tilesFound.size();i++){
            err<<","<<tilesFound[i];
        }
    }
    err<<"]: ";
    if(allGroup.size()!=3&&allGroup.size()!=4&&allGroup.size()!=6){
        err<<"size must be either 3, 4, or 6";
        throw err.str();
    }
    int max=tilesFound.size();
    if(max>4) max=4;//assert the first 3 or 4 tiles are sequential,
    //forcing the duplicate to the last 2
    int num=tilesFound[0]->getNumber();
    TileColor lastColor=tilesFound[0]->getTileColor();
    std::string error="";
    for(int i=1;i<max;i++){
        if(tilesFound[i]->getNumber()!=num){
            error="numbers must match";
            break;
        }
        if(tilesFound[i]->getTileColor()<=lastColor){
            error="the non duplicate colors must be sequential";
            break;
        }
        lastColor=tilesFound[i]->getTileColor();
    }
    if(error!=""){
        err<<error;
        throw err.str();
    }
}
std::vector<std::vector<unsigned int>> MaxGroup::getGroupForPossibilityKey(unsigned int possibilityKey){
    //TODO: this hasn't been updated to handle the 6 group
    std::vector<unsigned int> group;
    //last is all used
    if(possibilityKey==possibilityCount-1){
        group=allGroup;
    }
    else if(possibilityKey!=0){
        for(unsigned int i=0;i<allGroup.size();i++){
            //omit the selected which went in the unused
            if(i!=possibilityKey-1){
                group.push_back(allGroup[i]);
            }
        }
    }
    //else key==0, 0 is all unused, so return empty group
    std::vector<std::vector<unsigned int>> result;
    result.push_back(group);
    return result;
}
void MaxGroup::addCurrentUnused(int addTo[],int& addLocation){
    if(selected==0){
        for(unsigned int i=0;i<allGroup.size();i++){
            addTo[addLocation]=allGroup[i];
            addLocation++;
        }
    }
    else{
        //if(possibilityCount==2) done, using all tiles
        //1-4= return that tile as unused
        //else possibility #6: selected==5 and so using all the tiles, no add
        if(selected<5){
            addTo[addLocation]=allGroup[selected-1];
            addLocation++;
        }
        //selected==5 same as 4 tile case (using the first 4), but add the two dups to unused
        //if we have the 2 dups add them unless all used
        if(possibilityCount==7&&selected!=6){
            addTo[addLocation]=allGroup[4];
            addLocation++;
            addTo[addLocation]=allGroup[5];
            addLocation++;
        }
    }
}
unsigned int MaxGroup::getCurrentPossibilityKey(){
    return selected;
}
bool MaxGroup::isAtLast(){
    return selected==possibilityCount-1;
}
void MaxGroup::resetIteration(){
    selected=0;
}
void MaxGroup::moveNext(){
    selected++;
}
//allow sorting by group number
bool MaxGroup::operator<(const MaxGroup& other){
    //groups have all the same number;
    return allGroup[0]<other.allGroup[0];
}
bool MaxGroup::pointerLess(MaxGroup* r,MaxGroup* l)
{
    return *r<*l;
}
//unsigned int MaxGroup::allGroupSize(){
    //return allGroup.size();
//}
unsigned int MaxGroup::getPossibilityCount(){
    return possibilityCount;
}
std::string MaxGroup::debugString(){
    std::stringstream s;
    s<<"("<<selected<<"/"<<possibilityCount-1<<")[";
    if(allGroup.size()>0)
    {
        s<<FastCalcTile::Printable(allGroup[0]).toString();
        for(unsigned int i=1;i<allGroup.size();i++){
            s<<","<<FastCalcTile::Printable(allGroup[i]).toString();
        }
    }
    s<<"]"<<std::endl;
    return s.str();
}
