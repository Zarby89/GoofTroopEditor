lorom

;HOOKS
org $82888E
LDA #$1A ;Bell SFX
JSL $809A53 ;Play SFX
LDA #$04 : STA $04
RTS




org $829B2C ;LDA $6A : STA $32
JSL NewMessageCompare

;CODES
NewMessageCompare:
LDA $6A : STA $32
AND #$0F
CMP #$06 : BEQ .bookcheck
CMP #$04 : BEQ .bookcheck
RTL
.bookcheck
LDA $61 : AND #$40 : BEQ .noBook ;Y Pressed for Player 1 or 2
LDA $42 : CMP #$0C : BEQ .haveBook
LDA $43 : CMP #$0C : BNE .noBook
.haveBook
INC $00FB
INC $32 : INC $32
.noBook
RTL