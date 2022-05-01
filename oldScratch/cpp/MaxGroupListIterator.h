#ifndef MAXGROUPLISTITERATOR
#define MAXGROUPLISTITERATOR
//MaxGroupListIterator.h: 
//Created: Mon Sep 27 22:28:11 CDT 2021
#include "MaxGroup.h"

class MaxGroupListIterator
{
    public:
        MaxGroupListIterator(const std::vector<MaxGroup*>* groups);
        Tile* current();
        unsigned int size();
        bool moveNext();
    private:
        Tile* selected;
        unsigned int sizeValue;
        unsigned int groupNdx;
        unsigned int unusedNdx;
        const std::vector<MaxGroup*>* groups;

};
#endif//MAXGROUPLISTITERATOR
