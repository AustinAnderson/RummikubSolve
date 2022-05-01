CXXFLAGS=-std=c++11 -g
LDFLAGS=
oDir=obj/
oFiles=RunScorer.o Solver.o Tile.o TileColor.o MaxGroupListIterator.o MaxGroup.o
all: $(oDir)
	make --no-print-directory unclear
	make --no-print-directory comp
	make --no-print-directory clear
$(oDir):
	mkdir $(oDir)
comp: $(oFiles)
	g++ --std=c++11 -o bin/solver $(oFiles) $(LDFLAGS)
RunScorer.o: RunScorer.cpp RunScorer.h Tile.h TileColor.h MaxGroup.h \
 MaxGroupListIterator.h
Solver.o: Solver.cpp Tile.h TileColor.h RunScorer.h MaxGroup.h \
 MaxGroupListIterator.h
Tile.o: Tile.cpp Tile.h TileColor.h
TileColor.o: TileColor.cpp TileColor.h
MaxGroupListIterator.o: MaxGroupListIterator.cpp MaxGroupListIterator.h \
 MaxGroup.h Tile.h TileColor.h
MaxGroup.o: MaxGroup.cpp MaxGroup.h Tile.h TileColor.h
unclear:
	@if ! mv $(oDir)*.o .; then printf "";fi
clear:
	@if ! mv *.o $(oDir); then printf "";fi
clean:
	rm *.o ;rm $(oDir)*.o
