
;DESCRIPTION
;Make the platform sprite move differently
;it can only move in 3 time with vanilla code
;Direction are
;UP = 0
;RIGHT = 1
;DOWN = 2
;LEFT = 3
;TIME is number of frames waiting before next move
;Waiting_Time is the time before the platform start moving



!Move1_DIR = $00
!Move1_TIME = $38

!Move2_DIR = $03
!Move2_TIME = $50

!Move3_DIR = $02
!Move3_TIME = $38

!Up_SPEED = -2
!Right_SPEED = 2
!Down_SPEED = 2
!Left_SPEED = -2

!Waiting_Time = #$20


org $83A585
db !Move1_DIR, !Move1_TIME
db !Move2_DIR, !Move2_TIME
db !Move3_DIR, !Move3_TIME


db $00, $00, $00, !Up_SPEED ;UP
db $00, !Right_SPEED, $00, $00 ;RIGHT
db $00, $00, $00, !Down_SPEED ;DOWN
db $00, !Left_SPEED, $00, $00 ;LEFT

org $81C481
LDA !Waiting_Time

org $81C4BF
LDA !Waiting_Time