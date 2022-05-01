#ifndef RUNSCORER
#define RUNSCORER
#include <limits.h>
#include "FastCalcTile.h"
struct RunCalcState{
    RunCalcState();
    bool chainIsRunBreakingIfUnused[4];
    int chainScore[4];
    int lastForColor[4];
    std::string debugPrint();
};
class RunScorer {
    public:
        static int score(const int sortedList1[],int sortedList1Size,const int sortedList2[], int sortedList2Size);
        static std::string debugTileIntListStr(int tiles[],unsigned int s);
    private:
        static bool updateBreaking(int& score,int current, RunCalcState& state);
};
#endif//RUNSCORER
