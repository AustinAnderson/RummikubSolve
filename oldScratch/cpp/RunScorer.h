#ifndef RUNSCORER
#define RUNSCORER
#include "Tile.h"
#include "MaxGroup.h"
#include "MaxGroupListIterator.h"
#include <vector>
#include <limits.h>
#include <sstream>
//RunScorer.h: 
//Created: Sun Sep 26 16:05:32 CDT 2021
class RunScorer {

    public:
        RunScorer(const std::vector<MaxGroup*>* groups,const std::vector<Tile*>& groupBaseUnused);
        ~RunScorer();
        int score();
        static void allocateScoreData();
        static void deallocateScoreData();
    private:
        int getColorIndex(const Tile* tile);
        //106 total tiles in the game, so the unused will always be less than that
        static const unsigned int ScoreDataMemSize=106;
        static std::array<int,4> currentCount;
        static std::array<int,4> lastNumber;
        static std::array<int,4> lastLastNumber;
        static std::array<int*,4> scoreData;
        Tile** sortedUnused;
        unsigned int sortedUnusedLength;
};
#endif//RUNSCORER
