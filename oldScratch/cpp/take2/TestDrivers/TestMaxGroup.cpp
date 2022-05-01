#include <iostream>
#include "../MaxGroup.h"
#include "../FastCalcTile.h"
#include "../Tile.h"
#include "../TileColor.h"
#include <vector>
using namespace std;
void printTile(int t){
    if(t==0){
        cout<<"null";
    }
    else{
        cout<<FastCalcTile::Printable(t).toString();
    }
}
void printAddedTo(int addedTo[],int size){
    if(size>0){
        cout<<"[";
        printTile(addedTo[0]);
        for(int i=1;i<size;i++){
            cout<<",";
            printTile(addedTo[i]);
        }
        cout<<"]";
    }
}
void printAddedTo(unsigned int addedTo[],int size){
    printAddedTo(((int*)addedTo),size);
}
bool checkAddCurrentAndMoveNext(int addedTo[],int& index,MaxGroup& g)
{
    printAddedTo(addedTo,10);
    cout<<" i now "<<index<<endl;;
    cout<<"call add current"<<endl;
    g.addCurrentUnused(addedTo,index);
    printAddedTo(addedTo,10);
    cout<<" i now "<<index<<endl;;
    cout<<"is at last? "<<(g.isAtLast()?"true":"false")<<endl;
    bool ret=false;
    if(!g.isAtLast()){
        cout<<"moveNext"<<endl;
        g.moveNext();
        ret=true;
    }
    cout<<endl<<endl;
    return ret;
}
int main(){

    vector<Tile*> tiles={
        new Tile("3B"),
        new Tile("3R"),
        new Tile("3T"),
        new Tile("3Y")
    };
    MaxGroup g(tiles);
    
    int addedTo[10]={0,0,0,0,0,0,0,0,0,0};
    int index=0;
    bool done=false;
    for(int checked=0;checked<10&&!done;checked++){
        for(int i=0;i<10;i++){addedTo[i]=0;}
        done=!checkAddCurrentAndMoveNext(addedTo,index,g);
    }
    cout<<"group for possibility 4"<<endl;
    auto vec=g.getGroupForPossibilityKey(4);
    printAddedTo(vec.data(),vec.size());
    cout<<endl;

    for(unsigned int i=0;i<tiles.size();i++){
        delete tiles[i];
    }

    tiles={
        new Tile("3B"),
        new Tile("3R"),
        new Tile("3T"),
        new Tile("3Y"),
        new Tile("3B"),
        new Tile("3T")
    };
    g=MaxGroup(tiles);
    index=0;
    done=false;
    for(int checked=0;checked<10&&!done;checked++){
        for(int i=0;i<10;i++){addedTo[i]=0;}
        done=!checkAddCurrentAndMoveNext(addedTo,index,g);
    }
    cout<<"group for possibility 4"<<endl;
    vec=g.getGroupForPossibilityKey(4);
    printAddedTo(vec.data(),vec.size());
    cout<<endl;

    for(unsigned int i=0;i<tiles.size();i++){
        delete tiles[i];
    }
}
