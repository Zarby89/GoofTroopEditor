lorom

;HOOKS
org $82888E
NewBell:
LDA #$02 : STA $04             
LDA $47                  
CLC                      
ADC $0B                  
TAX ; x = 0C down, 0A right,  
LDY $BA1F,X              
LDA $BA20,X              
JSL $8089D7




RTS

org $8288B3
JSL $8089DE              
LDA $08 : CMP #$70                 
BNE +             
LDA #$04                 
STA $04
+
JSL CheckSpawnBall



RTS

org $8391E2
; fix speedtable?
dw $0000
dw $FE40

org $83FE00
animationData:
db $01, $04, $5B, $20

org $80D165
; new animation data address
LDA.b #(animationData>>8)
LDY.b #(animationData)


;CODES
CheckSpawnBall:
LDA $08
CMP #$7E : BNE +
JSL $808F8E ; get empty sprite slot
BCS +           
JSL CanonCode     
+
RTL

CanonCode:
LDA #$03                 
STA $0000,X              
STA $0001,X              
STZ $0003,X              
STZ $0002,X              
STZ $000B,X              
LDA #$12                 
STA $000A,X ; sprite ID



LDA $0147 : LSR
STA $000D,X       ; Direction 0 = up, 1 = right, 2 down, 3 left

CMP.b #$00 : BNE +
	LDA $0114 : SEC : SBC.b #$08
	BRA .setY
+
CMP.b #$02 : BNE +
	LDA $0114 : CLC : ADC.b #$08
	BRA .setY
+
LDA $0114
.setY
STA $0014,X              
STZ $0015,X

LDA $0147 : LSR
CMP.b #$03 : BNE +
	LDA $0111 : SEC : SBC.b #$0B
	BRA .setX
+
CMP.b #$01 : BNE +
	LDA $0111 : CLC : ADC.b #$0B
	BRA .setX
+
LDA $0111
.setX
STA $0011,X              
STZ $0012,X     

  

RTL





