lorom

org $8BFBD4
DemoRecorder1:
LDA #$04 ;Loop in here
STA $02 ;1122 or 1132 for player2
LDA $0E
REP #$10    
LDX $0C     
STA !Pos, X
LDA !Pos+1, X : INC : STA !Pos+1, X
SEP #$10    
RTL

DemoRecorder2:
LDA $0E     
REP #$10    
LDX $0C     
CMP !Pos,X 
BNE DemoRecorder3   
LDA !Pos+1, X 
INC         
BEQ DemoRecorder3  
STA !Pos+1, X
SEP #$10
RTL

DemoRecorder3:
INX
INX
LDA $0E
STA !Pos, X
LDA !Pos+1, X : INC : STA !Pos+1, X
STX $0C
SEP #$10
RTL

DemoClearer:
LDA #$00 
LDY #$00 
REP #$10 
LDX $0C  
--
STA $7FF800,X
INX
INY
CPY #$0100   
BNE -- 
SEP #$10 
RTL


