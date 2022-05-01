#include "MaxGroupListIterator.h"
MaxGroupListIterator::MaxGroupListIterator(const std::vector<MaxGroup*>* groups){
    sizeValue=0;
    this->groups=groups;
    for(unsigned int i=0;i<groups->size();i++){
        //         get vector<MaxGroup>
        //          |       get MaxGroup pointer at i
        //          |       |     get current iterator for dereferrenced MaxGroup@i
        //          |       |     |   get GroupIteration* pointed to by iterator
        //      ____|_______|_____|__/     getaddToUnused of dereferenced GroupIteration*
        //     /    |       |     |        |
        //     v    v       v     v        v
        sizeValue+=(*((*groups)[i]->current()))->addToUnused.size();
    }
    groupNdx=0;
    unusedNdx=0;
    if((*((*groups)[groupNdx]->current()))->addToUnused.size()==0){
        moveNext();
    }
}
Tile* MaxGroupListIterator::current(){
    return selected;
}
/*
   {{},{1},{2},{2,4}}
   {{},{1},{2},{2,4}}
   {{},{1},{2},{2,4}}
   {{},{1},{2},{2,4}}
 */
bool MaxGroupListIterator::moveNext(){
    Tile* old=selected;
    while(old==selected){
        if(groupNdx<groups->size()){
            if(unusedNdx<(*((*groups)[groupNdx]->current()))->addToUnused.size()){
                selected=(*((*groups)[groupNdx]->current()))->addToUnused[unusedNdx];
                unusedNdx++;
            }
            else{
                unusedNdx=0;
            }
            groupNdx++;
        }
        else{
            return false;
        }
    }
    return true;
}
unsigned int MaxGroupListIterator::size(){
    return sizeValue;
}
