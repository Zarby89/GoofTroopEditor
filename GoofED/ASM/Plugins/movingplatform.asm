
;DESCRIPTION
;Make the platform sprite move differently
;it can only move in 3 time with vanilla code
;Direction are
;UP = 0
;RIGHT = 1
;DOWN = 2
;LEFT = 3

;HOOKS

;Directions
org $83A585
;  DIR  LENGTH
db $00, $38 ; Move 1
db $03, $50 ; Move 2
db $02, $38 ; Move 3

;Speeds, change the -1 and 1 into higher values to make it go faster
db $00, $00, $00, -1 ; UP
db $00, 1, $00, $00  ; RIGHT
db $00, $00, $00, 1  ; DOWN
db $00, -1, $00, $00 ; LEFT

org $81C481
LDA #$7F ; TIMER waiting when platform reach the end

org $81C4BF
LDA #$7F ;T IMER waiting when platform reach the start