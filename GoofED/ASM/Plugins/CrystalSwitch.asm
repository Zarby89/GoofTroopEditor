;This plugin is adding a crystal switch and pegs from alttp (NO GFX)
;GFX Must be handled by the users the parameters are used to set tile8 used for switch
;You are limited to 15 peg tile per screen including the switch! (use to 14 to be safe)
;By default your Tile16 should have RED UP and BLUE DOWN

;HOOKS
!CS_BlueTileDownUL = $015B
!CS_BlueTileDownUR = $415B
!CS_BlueTileDownBL = $815B
!CS_BlueTileDownBR = $C15B

!CS_BlueTileUpUL = $016C
!CS_BlueTileUpUR = $016D
!CS_BlueTileUpBL = $017C
!CS_BlueTileUpBR = $017D

!CS_RedTileDownUL = $0177
!CS_RedTileDownUR = $4177
!CS_RedTileDownBL = $8177
!CS_RedTileDownBR = $C177

!CS_RedTileUpUL = $018C
!CS_RedTileUpUR = $018D
!CS_RedTileUpBL = $019C
!CS_RedTileUpBR = $019D

!CS_BlueSwitchTileUL = $012E
!CS_BlueSwitchTileUR = $012F
!CS_BlueSwitchTileBL = $013E
!CS_BlueSwitchTileBR = $013F

!CS_RedSwitchTileUL = $0244
!CS_RedSwitchTileUR = $0245
!CS_RedSwitchTileBL = $0254
!CS_RedSwitchTileBR = $0255

org $80A08B
JSL CrystalSwitchTransition

org $80DA0B
JSL CrystalSwitchTrigger

org $8392CC
db $02  ;Make the switch touchable by the hookshot

org $83933B
db $06, $04, $06, $06 ;Make the switch touchable by the hookshot

org $80DAAE
JSL CrystalSwitchHookshot

;CODES
UpdateSwitch:
LDA $7FFF24 ; 00 = RED UP / 01 = BLUE UP
EOR #$01
STA $7FFF24 
RTS

CrystalSwitchTrigger:
LDA $000A, X : CMP #$2A : BNE +
PHX
JSR UpdateSwitch
JSR .nochange
;Restored Code
SEP #$20
PLX
PLA : PLA : PLA ;pop the rtl
JML $80DA24
+
LDA $0B  
CMP #$0A 

RTL


.nochange
PHB : PHK : PLB

REP #$30
LDX #$0000

loopTiles:
LDA $7FE000, X : CMP #!CS_BlueTileDownUL : BNE + ; Blue tile down

LDA #$8080 ; Collision
LDY #$0008 ; Tile id
JMP UpdateTile
+
LDA $7FE000, X : CMP #!CS_RedTileUpUL : BNE + ; Red tile up
LDA #$0000 ; Collision
LDY #$0010
JMP UpdateTile
+
LDA $7FE000, X : CMP #!CS_RedTileDownUL : BNE + ; red tile down
LDA #$8080 ; Collision
LDY #$0018
JMP UpdateTile
+
LDA $7FE000, X : CMP #!CS_BlueTileUpUL : BNE + ; Blue tile up
LDA #$0000 ; Collision
LDY #$0000
JMP UpdateTile
+
LDA $7FE000, X : CMP #!CS_RedSwitchTileUL : BNE + ; crystal switch red
LDA #$F0F0 ; Collision
LDY #$0028
JMP UpdateTile
+
LDA $7FE000, X : CMP #!CS_BlueSwitchTileUL : BNE + ; crystal switch blue
LDA #$F0F0 ; Collision
LDY #$0020
JMP UpdateTile
+

increaseTile:
INX
INX
CPX #$0800 : BCC loopTiles


PLB
RTS





UpdateTile:
PHA
LDA .tiles, Y : STA $7FE000, X  ; set tilemap8
LDA .tiles+2, Y : STA $7FE002, X  ; set tilemap8
LDA .tiles+4, Y : STA $7FE040, X  ; set tilemap8
LDA .tiles+6, Y : STA $7FE042, X  ; set tilemap8

TXA : STA $7FFF20 : LSR : TAX
PLA
STA $1400, X ; tilemapcollision
STA $1420, X ; tilemapcollision

LDA $0041 ;load current position in the dma array
AND #$00FF : TAX
LDA #$0080 
STA $1900,X  ; mode
STA $1908,X  ; mode

LDA $7FFF20 : LSR
CLC
ADC #$5000
STA $1901, X ; pos
CLC
ADC #$0020
STA $1909, X ; pos

LDA #$0004
STA $1903, X ; length
STA $190B, X ; length

LDA .tiles, Y 
STA $1904,X
LDA .tiles+2, Y 
STA $1906,X

LDA .tiles+4, Y 
STA $190C,X
LDA .tiles+6, Y 
STA $190E,X

SEP #$31
TXA
ADC #$0F
STA $0041

REP #$30
LDA $7FFF20 : TAX

JMP increaseTile


.tiles
dw !CS_BlueTileDownUL, !CS_BlueTileDownUR, !CS_BlueTileDownBL, !CS_BlueTileDownBR ; 0x00 Blue Tile down
dw !CS_BlueTileUpUL, !CS_BlueTileUpUR, !CS_BlueTileUpBL, !CS_BlueTileUpBR ; 0x08 Blue Tile up
dw !CS_RedTileDownUL, !CS_RedTileDownUR, !CS_RedTileDownBL, !CS_RedTileDownBR ; 0x10 Red Tile down
dw !CS_RedTileUpUL, !CS_RedTileUpUR, !CS_RedTileUpBL, !CS_RedTileUpBR ; 0x18 Red Tile up
dw !CS_RedSwitchTileUL, !CS_RedSwitchTileUR, !CS_RedSwitchTileBL, !CS_RedSwitchTileBR ; 0x20 red crystal switch
dw !CS_BlueSwitchTileUL, !CS_BlueSwitchTileUR, !CS_BlueSwitchTileBL, !CS_BlueSwitchTileBR ; 0x28 blue crystal switch


CrystalSwitchTransition:
LDA $7FFF24 : AND #$01 : BNE +

JSR CrystalSwitchTrigger_nochange

SEP #$30

+
JSL $82C17C 

RTL


CrystalSwitchHookshot:
LDA $000A,Y : CMP #$2A : BNE +
PHY
JSR UpdateSwitch

JSR CrystalSwitchTrigger_nochange
PLY
SEP #$20

+
STZ $1C  
LDA #$00 

RTL