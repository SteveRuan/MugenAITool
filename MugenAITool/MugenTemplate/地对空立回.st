;---------------------------------------------------------------------------
[State -1, {actionName}]
type = ChangeState
value = {actionStateN}
Triggerall = !Ishelper && EnemyNear,Alive
Triggerall = Var(59) > 0
Triggerall = Roundstate = 2
Triggerall = !inguarddist
Triggerall = Statetype != A
Triggerall = EnemyNear,Stateno != [5120,5220)
Triggerall = !((EnemyNear,Statetype = S && (EnemyNear,Const(size.head.pos.y) > {actionHeightLow})) || (EnemyNear,Statetype = C && (EnemyNear,Const(size.mid.pos.y) > {actionHeightLow})))
Triggerall = !(Ceil(Helper(97530),FVar(39)) & (2**1 + 2**2 + 2**4))
Triggerall = Helper(97530),FVar(37) >= {actionJuggle}
Trigger1 = (((P2bodydist X-EnemyNear,Vel X*{actionTime} = [0,({actionFrontDist}-8)+(Stateno = [100,101])*5]) && P2dist X >= 0) || (P2bodydist X < ({actionFrontDist}-3)+(Stateno = [100,101])*5 && P2dist X >= 0))
Trigger1 = !(P2bodydist X+EnemyNear,Vel X*{actionTime} > {actionFrontDist}+(Stateno = [100,101])*5 && EnemyNear,Statetype = A && facing = EnemyNear,facing)
Trigger1 = ((EnemyNear,Pos Y + EnemyNear,Vel Y * {actionTime} + EnemyNear,const(movement.yaccel) * {actionTime} * ({actionTime} + 1) / 2 > {actionHeightHigh}) && EnemyNear,Statetype = A && EnemyNear,Vel Y >= 0) || EnemyNear,Statetype != A
Trigger1 = EnemyNear,Statetype != L && EnemyNear,Movetype = I
Trigger1 = EnemyNear,Numproj = 0
Trigger1 = +++ || ((EnemyNear,Pos Y + EnemyNear,Vel Y * {actionTime} + EnemyNear,const(movement.yaccel) * {actionTime} * ({actionTime} + 1) / 2 < 0) && EnemyNear,Statetype = A) ;(Helper(97530),FVar(31) = []) 
Trigger1 = (ctrl || (Stateno = [19,21]) || (Stateno = [100,101]))
