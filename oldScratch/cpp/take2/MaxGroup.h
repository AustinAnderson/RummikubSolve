#ifndef MAXGROUP
#define MAXGROUP
#include <vector>
#include "Tile.h"
//MaxGroup.h: 
//Created: Sat Sep 25 18:50:00 CDT 2021

class MaxGroup {
    public:
        //need to update to be 3, 4, or '6' group that shows up as two 3s or one 4
        MaxGroup(const std::vector<Tile*>& tilesFound);
        MaxGroup(const MaxGroup& toCp)=delete;
        std::vector<std::vector<unsigned int>> getGroupForPossibilityKey(unsigned int possibilityKey);
        //addTo size is assumed to be > addLocation + allGroupSize()
        void addCurrentUnused(int addTo[],int& addLocation);
        unsigned int getCurrentPossibilityKey();
        unsigned int getPossibilityCount();
        bool isAtLast();
        void moveNext();
        void resetIteration();
        bool operator<(const MaxGroup& other);
        static bool pointerLess(MaxGroup* r,MaxGroup* l);
        std::string debugString();
    private:
        unsigned int possibilityCount;
        std::vector<unsigned int> allGroup;
        unsigned int selected;
};
#endif//MAXGROUP
