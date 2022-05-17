hi RTile ctermfg=Red guifg=Red
hi TTile ctermfg=Cyan guifg=Cyan
hi YTile ctermfg=Yellow guifg=Yellow
match RTile /[^ BTY\t]*R[^ BTY\t]*/
2match TTile /[^ RBY\t]*T[^ RBY\t]*/
3match YTile /[^ RTB\t]*Y[^ RTB\t]*/
