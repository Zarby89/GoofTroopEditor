;THIS PLUGIN DO NOT HAVE PARAMETERS SINCE IT CAN BE VERY DIFFERENT
;ON EVERY HACK, You can change it by writing new code directly
;in the plugin file examples are below you can just copy paste them for more
;if you want more map you just copy the section between the ----------------
;in the LEVEL you want, if you want multiple maps in the same level
;you just copy it right under it and change the room id to the room you want

;HOOKS
org $80DDDF
JSL NewIslanderCode
NOP #01

;CODES
NewIslanderCode:
SEP #$10
;Here is the code that check what level and map we are on!
LDA $00B6 ; Load Level Index
CMP #$00 : BNE ++ ; Are we on level 1 (00)?
;Level 1 Maps go here


++
CMP #$01 : BNE ++ ; Are we on level 2 (01)?
;Level 2 Maps go here


++
CMP #$02 : BNE ++ ; Are we on level 3 (02)?
;Level 3 Maps go here

;----------------------------------------------------------------------
LDA $00B7 ;Load map index ;<- example here for the vanilla castle
CMP #$0F : BNE +
JSL $808F1E ;KillSprite we remove the islander if map is 0F in level 3
+
;----------------------------------------------------------------------



++
CMP #$03 : BNE ++ ; Are we on level 4 (03)?
;Level 4 Maps go here


++
CMP #$04 : BNE ++ ; Are we on level 5 (04)?
;Level 5 Maps go here


++

LDA $00C7 ; Restore original code
RTL ; Jump back to normal code
