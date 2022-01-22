org $80B70B
JML NewTile32Load
NOP #01

org $80B710
ReturnTileLoad:

org $80B56F
JML NewTile32Load2
NOP #01

org $80B574
NewTile32Load2Return:


org $8AFE30
NewTile32Load:
STA $16
LDA $B6 : AND #$00FF : CMP #$0003 : BCS +
PEA $898A
JML ReturnTileLoad
+
PEA $8992
JML ReturnTileLoad


NewTile32Load2:
STA $16
LDA $B6 : AND #$00FF : CMP #$0003 : BCS +
PEA $8B8A
JML NewTile32Load2Return
+
PEA $8B92
JML NewTile32Load2Return

warnpc $8AFFFF