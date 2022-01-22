lorom

org $80A8CC; ice routine A8E9=RTS  ;Length #29
NewIceRoutine:
{
    
STZ $CA ; Set ice on off
LDX $B6 ; Load level
LDA $B7 ; load map id
CLC : ADC.l LevelOffsets, X
TAX
LDA.l LevelOffsets, X : AND #$01 : BEQ .noIce
INC $CA ;Set Ice on ON
.noIce
RTS
 
org $80A80E ;to 80A81F ;On map load routine check if room is dark or not
LDA.l LevelOffsets, X  : AND #$02 : BNE .darkRoom ;<- Dark Room Routine thing
NOP #11
}
 
org $80A823 ;If we find a dark room
.darkRoom

org $89F055

LevelOffsets:
db $05, $15, $25, $3F, $5D