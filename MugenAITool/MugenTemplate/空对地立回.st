;---------------------------------------------------------------------------
[State -1, {actionName}]
type = ChangeState
value = {actionStateNo}
Triggerall = !Ishelper && EnemyNear,Alive
Triggerall = Var(59) > 0
Triggerall = Roundstate = 2
Triggerall = !inguarddist
Triggerall = Statetype = A
Triggerall = EnemyNear,Stateno != [5120,5220)
Triggerall = !((EnemyNear,Statetype = S && (EnemyNear,Const(size.head.pos.y) > {actionHeightLow})) || (EnemyNear,Statetype = C && (EnemyNear,Const(size.mid.pos.y) > {actionHeightLow})))
Triggerall = !(Ceil(Helper(97530),FVar(39)) & (2**1 + 2**2))
Triggerall = Helper(97530),FVar(37) >= {actionJuggle}
Triggerall = (Pos Y + Vel Y * {actionTime} + Const(movement.yaccel) * {actionTime} * ({actionTime}+1) / 2 < 0) && Vel Y > 0
Triggerall = ctrl && +++ ;(Helper(97530),FVar(31) > 1000-Var(59)*160)
Triggerall = ((P2dist Y - Vel Y * ({actionTime}+random%{actionpersisttime}) - const(movement.yaccel) * ({actionTime}+random%{actionpersisttime}) * (({actionTime}+random%{actionpersisttime}) + 1) / 2 + EnemyNear(Var(54)),Vel Y * ({actionTime}+random%{actionpersisttime}) + EnemyNear(Var(54)),const(movement.yaccel) * ({actionTime}+random%{actionpersisttime}) * (({actionTime}+random%{actionpersisttime}) + 1) / 2 = [{actionHeightHigh}-40,{actionHeightLow}-80}])
Triggerall = EnemyNear(Var(54)),Statetype = A
Triggerall = (P2bodydist X-Vel X*({actionTime}+random%{actionpersisttime})-EnemyNear(Var(54)),Vel X*({actionTime}+random%{actionpersisttime}) = [-5,{actionFrontDist}])
Triggerall = EnemyNear(Var(54)),Statetype != L && EnemyNear(Var(54)),Movetype = I

;---------------------------------------------------------------------------
[State -1, {actionName}]
type = ChangeState
value = {actionStateNo}
Triggerall = !Ishelper && EnemyNear,Alive
Triggerall = Var(59) > 0
Triggerall = Roundstate = 2
Triggerall = !inguarddist
Triggerall = Statetype = A
Triggerall = EnemyNear,Stateno != [5120,5220)
Triggerall = !((EnemyNear,Statetype = S && (EnemyNear,Const(size.head.pos.y) > {actionHeightLow})) || (EnemyNear,Statetype = C && (EnemyNear,Const(size.mid.pos.y) > {actionHeightLow})))
Triggerall = !(Ceil(Helper(97530),FVar(39)) & (2**1 + 2**2))
Triggerall = Helper(97530),FVar(37) >= {actionJuggle}
Triggerall = (Pos Y + Vel Y * {actionTime} + Const(movement.yaccel) * {actionTime} * ({actionTime}+1) / 2 < 0) && Vel Y > 0
Triggerall = ctrl && +++ ;(Helper(97530),FVar(31) > 1000-Var(59)*160)
Triggerall = ((Pos Y + Vel Y * ({actionTime}+random%{actionpersisttime}) + Const(movement.yaccel) * ({actionTime}+random%{actionpersisttime}) * (({actionTime}+random%{actionpersisttime})+1) / 2 >= 0) && (EnemyNear(Var(54)),Statetype = S || EnemyNear(Var(54)),Statetype = C))
Triggerall = (P2bodydist X-Vel X*({actionTime}+random%{actionpersisttime})-EnemyNear(Var(54)),Vel X*({actionTime}+random%{actionpersisttime}) = [-5,{actionFrontDist}-5]) || (P2bodydist X < {actionFrontDist}-8 && P2dist X >= 0) || (Var(49) = [31,34])
