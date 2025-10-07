lorom

;HOOKS
org $80DDB5
JSL NewIslanderGfxCode ; override the Islander draw code

org $83FE00 ; some free space in bank 83
IslanderGfx:
.islander1
db $02, $7F, $07 : db $20, $28 ; try to reuse knight armor draw code ($07)
.islander2
db $02, $7F, $07 : db $22, $2A
.islander3
db $02, $7F, $07 : db $24, $2C
; can add more islanders here and swap tiles to mix head/body

warnpc $83FFFF ; throw a warning if we run out of space

;CODES
NewIslanderGfxCode:
LDA.b $0D ; load sprite direction param
BEQ .nospecialgfx
; Here direction is not 00 so load different sprite draw routine
CMP.b #$01 : BNE +
; if it's 01 then just go back to orig code as well
BRA .nospecialgfx
+
CMP.b #$02 : BNE +
LDY.b #IslanderGfx_islander1
LDA.b #(IslanderGfx_islander1>>8)
BRA .loadanim
+
CMP.b #$03 : BNE +
LDY.b #IslanderGfx_islander2
LDA.b #(IslanderGfx_islander2>>8)
BRA .loadanim
+
CMP.b #$04 : BNE +
LDY.b #IslanderGfx_islander3
LDA.b #(IslanderGfx_islander3>>8)
BRA .loadanim
+
; can add more conditions here to 

.nospecialgfx
LDA.b #$99 ; reload original value
.loadanim
JSL $8089A3 ; do the animation code
RTL ; go back to original code