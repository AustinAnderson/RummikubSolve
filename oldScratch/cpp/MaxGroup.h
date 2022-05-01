#ifndef MAXGROUP
#define MAXGROUP
#include <vector>
#include "Tile.h"
//MaxGroup.h: 
//Created: Sat Sep 25 18:50:00 CDT 2021

struct GroupIteration{
    GroupIteration();
    GroupIteration(std::vector<Tile*> group,std::vector<Tile*> addToUnused);
    std::vector<Tile*> group;
    std::vector<Tile*> addToUnused;
};
class MaxGroup {
    public:
        MaxGroup(std::vector<Tile*> tilesFound);
        MaxGroup(const MaxGroup& toCp)=delete;
        ~MaxGroup();
        const std::vector<GroupIteration*>::const_iterator current() const;
        bool isAtLast();
        void moveNext();
        void resetIteration();
        bool operator<(const MaxGroup& other);
        unsigned int size();
    private:
        std::vector<GroupIteration*> possibilities;
        std::vector<GroupIteration*>::const_iterator selected;
};
#endif//MAXGROUP
