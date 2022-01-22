org $829D08 ;JSL $808667 
JSL OverwritePlankDMA


org $829CEC ; LDY $B930,X : LDA #$FF
JSL OverwritePlankDMAPlacing
NOP #01


org $918000
OverwritePlankDMA:
JSL $808667 ;set the DMA Buffers ($41)
;position + 0x4FFF
LDY $0022 ;the current plank being read
LDA $0041 : SEC : SBC #$18 : TAX
REP #$30
LDA $8552,Y ;Load Tile Position
CLC : ADC #$4FFF
STA $1901,X
CLC : ADC #$0020
STA $190D,X
SEP #$30
RTL

OverwritePlankDMAPlacing:
STX $0022
LDY $8555, X
LDA #$FF
RTL



