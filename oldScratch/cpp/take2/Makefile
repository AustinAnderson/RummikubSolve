CXXFLAGS=--std=c++11 -g
LDFLAGS=
oDir=obj/
oFiles=RunScorer.o Solver.o Tile.o FastCalcTile.o TileColor.o MaxGroup.o
all: $(oDir)
	make --no-print-directory unclear
	make --no-print-directory comp
	make --no-print-directory clear
$(oDir):
	mkdir $(oDir)
comp: $(oFiles)
	g++ -g --std=c++11 -o bin/solver $(oFiles) $(LDFLAGS)
RunScorer.o: RunScorer.cpp RunScorer.h FastCalcTile.h TileColor.h
Solver.o: Solver.cpp Tile.h TileColor.h RunScorer.h FastCalcTile.h \
 MaxGroup.h
Tile.o: Tile.cpp Tile.h TileColor.h FastCalcTile.h
FastCalcTile.o: FastCalcTile.cpp FastCalcTile.h TileColor.h
TileColor.o: TileColor.cpp TileColor.h
MaxGroup.o: MaxGroup.cpp MaxGroup.h Tile.h TileColor.h
unclear:
	@if ! mv $(oDir)*.o .; then printf "";fi
clear:
	@if ! mv *.o $(oDir); then printf "";fi
clean:
	rm *.o ;rm $(oDir)*.o
