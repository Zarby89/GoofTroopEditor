lorom
org $809F95
JSL DebugCode


org $8AFE00
DebugCode:
LDA #$08 : STA $B7 ;Set MAP Index to 8
LDA #$02 : STA $B6 ;Set LEVEL
STZ $A8 ; ?
LDA #$60 : STA $014C
LDA #$60 : STA $014D

LDA #$60 : STA $01CC
LDA #$80 : STA $01CD


RTL ;31 bytes

org $809DE4
LDA #$04 ; Remove the intro sequence




