
;---------------------------------------------------------------------------
; GUARD (start)
[Statedef 120]
type = U    ;Leave state type unchanged
physics = U ;Leave physics unchanged

[State 120, 1]
type = ChangeAnim
trigger1 = Time = 0
value = 120 + (statetype = C) + (statetype = A)*2

[State 120, 2]
type = StateTypeSet
trigger1 = Time = 0 && statetype = S
physics = S

[State 120, 3]
type = StateTypeSet
trigger1 = Time = 0 && statetype = C
physics = C

[State 120, 4]
type = StateTypeSet
trigger1 = Time = 0 && statetype = A
physics = A

[State 120, Hi to Lo]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = S && command = "holddown"
statetype = C
physics = C

[State 120, Lo to Hi]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = C && command != "holddown"
statetype = S
physics = S

[State 120, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = C
physics = C

[State 120, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
trigger2 = (EnemyNear,Stateno = [200,300)) && (Var(49) = [1,25])
Trigger2 = P2bodydist X < 45 && EnemyNear,Time < 10 && EnemyNear,Time <= Helper(97531),FVar(Var(49))
statetype = C
physics = C

[State 120, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = !(EnemyNear,Statetype = S && (EnemyNear,Stateno = [200,300)) && P2bodydist X < 45 && EnemyNear,Time < 10)
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 120, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 120, 5]
type = ChangeState
trigger1 = AnimTime = 0
value = 130 + (statetype = C) + (statetype = A)*2

[State 120, Stop Guarding]
type = ChangeState
triggerall = !{VariableType}({VariableNo})
trigger1 = command != "holdback"
trigger2 = !inguarddist
value = 140

[State 120, Stop Guarding]
type = ChangeState
triggerall = {VariableType}({VariableNo})
trigger1 = !inguarddist
trigger1 = !(Helper(97531),Inguarddist && Helper(97531),Var(0) > 0)
trigger1 = !(P2bodydist X < 60 && EnemyNear,Statetype = A && EnemyNear,Movetype = A && EnemyNear,p2dist x < 0)
value = 140

;---------------------------------------------------------------------------
; STAND GUARD (guarding)
[Statedef 130]
type    = S
physics = S

[State 130, 1]
type = ChangeAnim
trigger1 = Anim != 130
value = 130

[State 130, Hi to Lo]
type = ChangeState
triggerall = !{VariableType}({VariableNo})
trigger1 = command = "holddown"
value = 131

[State 130, Hi to Lo]
type = ChangeState
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
value = 131

[State 130, Hi to Lo]
type = ChangeState
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
trigger2 = {VariableType}({VariableNo}) = 5
trigger2 = (EnemyNear,Stateno = [200,300)) && (Var(49) = [1,25])
Trigger2 = P2bodydist X < 45 && EnemyNear,Time < 10 && EnemyNear,Time <= Helper(97531),FVar(Var(49))
value = 131

[State 130, Stop Guarding]
type = ChangeState
triggerall = !{VariableType}({VariableNo})
trigger1 = command != "holdback"
trigger2 = !inguarddist
value = 140

[State 130, Stop Guarding]
type = ChangeState
triggerall = {VariableType}({VariableNo})
trigger1 = !inguarddist
trigger1 = !(Helper(97531),Inguarddist && Helper(97531),FVar(38) > 0)
trigger1 = !(P2bodydist X < 60 && EnemyNear,Statetype = A && EnemyNear,Movetype = A && EnemyNear,p2dist x < 0)
value = 140

;---------------------------------------------------------------------------
; CROUCH GUARD (guarding)
[Statedef 131]
type    = C
physics = C

[State 131, 1]
type = ChangeAnim
trigger1 = Anim != 131
value = 131

[State 131, Lo to Hi]
type = ChangeState
triggerall = !{VariableType}({VariableNo})
trigger1 = command != "holddown"
value = 130

[State 131, Lo to Hi]
type = ChangeState
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = !(EnemyNear,Statetype = S && (EnemyNear,Stateno = [200,300)) && P2bodydist X < 45 && EnemyNear,Time < 10)
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
value = 130

[State 131, Lo to Hi]
type = ChangeState
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
value = 130

[State 131, Stop Guarding]
type = ChangeState
triggerall = !{VariableType}({VariableNo})
trigger1 = command != "holdback"
trigger2 = !inguarddist
value = 140

[State 131, Stop Guarding]
type = ChangeState
triggerall = {VariableType}({VariableNo})
trigger1 = !inguarddist
trigger1 = !(Helper(97531),Inguarddist && Helper(97531),FVar(38) > 0)
trigger1 = !(P2bodydist X < 60 && EnemyNear,Statetype = A && EnemyNear,Movetype = A && EnemyNear,p2dist x < 0)
value = 140

;---------------------------------------------------------------------------
; AIR GUARD (guarding)
[Statedef 132]
type    = A
physics = N

[State 132, 1]
type = ChangeAnim
trigger1 = Anim != 132
value = 132

[State 132, 2]
type = VelAdd
trigger1 = 1
y = Const(movement.yaccel)

[State 132, 3]
type = VarSet
trigger1 = 1
sysvar(0) = (pos y >= 0) && (vel y > 0)

[State 132, 4]
type = VelSet
trigger1 = sysvar(0)
y = 0

[State 132, 5]
type = PosSet
trigger1 = sysvar(0)
y = 0

[State 132, 6]
type = ChangeState
triggerall = sysvar(0)
triggerall = inguarddist
trigger1 = !{VariableType}({VariableNo})
trigger1 = command = "holdback"
trigger2 = {VariableType}({VariableNo})
value = 130

[State 132, 7]
type = ChangeState
trigger1 = sysvar(0)
value = 52

[State 132, Stop Guarding]
type = ChangeState
triggerall = !{VariableType}({VariableNo})
trigger1 = command != "holdback"
trigger2 = !inguarddist
value = 140

[State 132, Stop Guarding]
type = ChangeState
triggerall = {VariableType}({VariableNo})
trigger1 = !inguarddist
trigger1 = !(Helper(97531),Inguarddist && Helper(97531),Var(0) > 0)
trigger1 = !(P2bodydist X < 60 && EnemyNear,Statetype = A && EnemyNear,Movetype = A && EnemyNear,p2dist x < 0)
value = 140

;---------------------------------------------------------------------------
; GUARD (end)
[Statedef 140]
type = U    ;Leave state type unchanged
physics = U ;Leave physics unchanged
ctrl = 1

[State 140, 1]
type = ChangeAnim
trigger1 = Time = 0
value = 140 + (statetype = C) + (statetype = A)*2

[State 140, 2]
type = StateTypeSet
trigger1 = Time = 0 && statetype = S
physics = S

[State 140, 3]
type = StateTypeSet
trigger1 = Time = 0 && statetype = C
physics = C

[State 140, 4]
type = StateTypeSet
trigger1 = Time = 0 && statetype = A
physics = A

[State 140, Hi to Lo]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = S && command = "holddown"
statetype = C
physics = C

[State 140, Lo to Hi]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = C && command != "holddown"
statetype = S
physics = S

;[State 140, 5] ;Implemented within engine
;type = ChangeState
;trigger1 = AnimTime = 0
;value = (statetype = C)*11 + (statetype = A)*51

;---------------------------------------------------------------------------
; SGUARDHIT (shaking)
[Statedef 150]
type    = S
movetype= H
physics = N
velset = 0,0

[State 150, 1]
type = ChangeAnim
trigger1 = 1
value = 150

[State 150, 2]
type = ChangeState
trigger1 = HitShakeOver
value = 151 + 2*(command = "holddown")

[State 150, Hi to Lo]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = S && command = "holddown"
statetype = C
physics = C

[State 150, Lo to Hi]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = C && command != "holddown"
statetype = S
physics = S

[State 150, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = C
physics = C

[State 150, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
trigger2 = {VariableType}({VariableNo}) = 5
trigger2 = (EnemyNear,Stateno = [200,300)) && (Var(49) = [1,25])
Trigger2 = P2bodydist X < 45 && EnemyNear,Time < 10 && EnemyNear,Time <= Helper(97531),FVar(Var(49))
statetype = C
physics = C

[State 150, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = !(EnemyNear,Statetype = S && (EnemyNear,Stateno = [200,300)) && P2bodydist X < 45 && EnemyNear,Time < 10)
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 150, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 150, 3]
type = ForceFeedback
trigger1 = time = 0
waveform = square
time = 3

;---------------------------------------------------------------------------
; SGUARDHIT2 (knocked back)
[Statedef 151]
type    = S
movetype= H
physics = S
anim = 150

[State 151, 1]
type = HitVelSet
trigger1 = Time = 0
x = 1

[State 151, 2]
type = VelSet
trigger1 = Time = GetHitVar(slidetime)
trigger2 = HitOver
x = 0

[State 151, 3]
type = CtrlSet
trigger1 = Time = GetHitVar(ctrltime)
value = 1

[State 151, Hi to Lo]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = S && command = "holddown"
statetype = C
physics = C

[State 151, Lo to Hi]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = C && command != "holddown"
statetype = S
physics = S

[State 151, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = C
physics = C

[State 151, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
trigger2 = {VariableType}({VariableNo}) = 5
trigger2 = (EnemyNear,Stateno = [200,300)) && (Var(49) = [1,25])
Trigger2 = P2bodydist X < 45 && EnemyNear,Time < 10 && EnemyNear,Time <= Helper(97531),FVar(Var(49))
statetype = C
physics = C

[State 151, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = !(EnemyNear,Statetype = S && (EnemyNear,Stateno = [200,300)) && P2bodydist X < 45 && EnemyNear,Time < 10)
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 151, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 151, 4]
type = ChangeState
trigger1 = HitOver
value = 130
ctrl = 1

;---------------------------------------------------------------------------
; CGUARDHIT (shaking)
[Statedef 152]
type    = C
movetype= H
physics = N
velset = 0,0

[State 152, 1]
type = ChangeAnim
trigger1 = 1
value = 151

[State 152, 3]
type = ChangeState
trigger1 = HitShakeOver
value = 151 + 2*(command = "holddown")

[State 152, Hi to Lo]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = S && command = "holddown"
statetype = C
physics = C

[State 152, Lo to Hi]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = C && command != "holddown"
statetype = S
physics = S

[State 152, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = C
physics = C

[State 152, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
trigger2 = {VariableType}({VariableNo}) = 5
trigger2 = (EnemyNear,Stateno = [200,300)) && (Var(49) = [1,25])
Trigger2 = P2bodydist X < 45 && EnemyNear,Time < 10 && EnemyNear,Time <= Helper(97531),FVar(Var(49))
statetype = C
physics = C

[State 152, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = !(EnemyNear,Statetype = S && (EnemyNear,Stateno = [200,300)) && P2bodydist X < 45 && EnemyNear,Time < 10)
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 152, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 152, 4]
type = ForceFeedback
trigger1 = time = 0
waveform = square
time = 4

;---------------------------------------------------------------------------
; CGUARDHIT2 (knocked back)
[Statedef 153]
type    = C
movetype= H
physics = C
anim = 151

[State 153, 1]
type = HitVelSet
trigger1 = Time = 0
x = 1

[State 153, 2]
type = VelSet
trigger1 = Time = GetHitVar(slidetime)
trigger2 = HitOver
x = 0

[State 153, 3]
type = CtrlSet
trigger1 = Time = GetHitVar(ctrltime)
value = 1

[State 153, Hi to Lo]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = S && command = "holddown"
statetype = C
physics = C

[State 153, Lo to Hi]
type = StateTypeSet
triggerall = !{VariableType}({VariableNo})
trigger1 = statetype = C && command != "holddown"
statetype = S
physics = S

[State 153, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = C
physics = C

[State 153, Hi to Lo]
type = StateTypeSet
triggerall = statetype = S
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
trigger2 = {VariableType}({VariableNo}) = 5
trigger2 = (EnemyNear,Stateno = [200,300)) && (Var(49) = [1,25])
Trigger2 = P2bodydist X < 45 && EnemyNear,Time < 10 && EnemyNear,Time <= Helper(97531),FVar(Var(49))
statetype = C
physics = C

[State 153, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = !(EnemyNear,Statetype = S && (EnemyNear,Stateno = [200,300)) && P2bodydist X < 45 && EnemyNear,Time < 10)
triggerall = EnemyNear,Statetype = S && EnemyNear,movetype = A
trigger1 = !(Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 153, Lo to Hi]
type = StateTypeSet
triggerall = statetype = C
triggerall = {VariableType}({VariableNo}) > 2 
triggerall = EnemyNear,Statetype = C && EnemyNear,movetype = A
trigger1 = (Ceil(Helper(97530),FVar(39)) & (2**9))
statetype = S
physics = S

[State 153, 4]
type = ChangeState
trigger1 = HitOver
value = 131
ctrl = 1

;---------------------------------------------------------------------------
; AGUARDHIT (shaking)
[Statedef 154]
type    = A
movetype= H
physics = N
velset = 0,0

[State 154, 1]
type = ChangeAnim
trigger1 = 1
value = 152

[State 154, 2]
type = ChangeState
trigger1 = HitShakeOver
value = 155 ;AGUARDHIT2

[State 154, 3]
type = ForceFeedback
trigger1 = time = 0
waveform = square
time = 4

;---------------------------------------------------------------------------
; AGUARDHIT2 (knocked away)
[Statedef 155]
type    = A
movetype= H
physics = N
anim = 152

[State 155, 1]
type = HitVelSet
trigger1 = Time = 0
x = 1
y = 1

[State 155, 2]
type = VelAdd
trigger1 = 1
y = Const(movement.yaccel)

[State 155, 3]
type = CtrlSet
trigger1 = Time = GetHitVar(ctrltime)
value = 1

[State 155, 4]
type = VarSet
trigger1 = 1
sysvar(0) = (pos y >= 0) && (vel y > 0)

[State 155, 5]
type = VelSet
trigger1 = sysvar(0)
y = 0

[State 155, 6]
type = PosSet
trigger1 = sysvar(0)
y = 0

[State 155, 6]
type = ChangeState
triggerall = sysvar(0)
triggerall = inguarddist
trigger1 = {VariableType}({VariableNo}) = 0
trigger1 = command = "holdback"
trigger2 = {VariableType}({VariableNo}) > 0
value = 130

[State 155, 7]
type = ChangeState
trigger1 = sysvar(0)
value = 52
