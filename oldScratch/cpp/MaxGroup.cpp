#include "MaxGroup.h"
GroupIteration::GroupIteration(){}
GroupIteration::GroupIteration(std::vector<Tile*> group,std::vector<Tile*> addToUnused){
    this->group=group;
    this->addToUnused=addToUnused;
}
MaxGroup::MaxGroup(std::vector<Tile*> tilesFound){
    possibilities.push_back(new GroupIteration(tilesFound,std::vector<Tile*>()));
    if(tilesFound.size()==4){
        possibilities.push_back(new GroupIteration(
            std::vector<Tile*>{tilesFound[0],tilesFound[1],tilesFound[2]},
            std::vector<Tile*>{tilesFound[3]}
        ));
        possibilities.push_back(new GroupIteration(
            std::vector<Tile*>{tilesFound[0],tilesFound[1],tilesFound[3]},
            std::vector<Tile*>{tilesFound[2]}
        ));
        possibilities.push_back(new GroupIteration(
            std::vector<Tile*>{tilesFound[0],tilesFound[1],tilesFound[3]},
            std::vector<Tile*>{tilesFound[1]}
        ));
        possibilities.push_back(new GroupIteration(
            std::vector<Tile*>{tilesFound[1],tilesFound[2],tilesFound[3]},
            std::vector<Tile*>{tilesFound[0]}
        ));
    }
    possibilities.push_back(new GroupIteration(std::vector<Tile*>(),tilesFound));
    selected=possibilities.begin();
}
MaxGroup::~MaxGroup(){
    for(unsigned int i=0;i<possibilities.size();i++){
        delete possibilities[i];
    }
}
const std::vector<GroupIteration*>::const_iterator MaxGroup::current() const {
    return selected;
}
bool MaxGroup::isAtLast(){
    return selected==(possibilities.end()-1);
}
void MaxGroup::resetIteration(){
    selected=possibilities.begin();
}
void MaxGroup::moveNext(){
    selected++;
}
//allow sorting by group number
bool MaxGroup::operator<(const MaxGroup& other){
    //groups have all the same number;
    //the first possibility is the full group with none unused
    return possibilities[0]->group[0]->getNumber()<other.possibilities[0]->group[0]->getNumber();
}
unsigned int MaxGroup::size(){
    return possibilities.size();
}
