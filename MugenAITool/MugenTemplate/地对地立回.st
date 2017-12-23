;---------------------------------------------------------------------------
[State -1, {actionName}]
type = ChangeState
value = {actionStateNo}
Triggerall = !Ishelper && EnemyNear,Alive
Triggerall = Var(59) > 0
Triggerall = Roundstate = 2
Triggerall = !inguarddist
Triggerall = Statetype != A
Triggerall = EnemyNear,Stateno != [5120,5220)
Triggerall = !((EnemyNear,Statetype = S && (EnemyNear,Const(size.head.pos.y) > {actionHeightLow})) || (EnemyNear,Statetype = C && (EnemyNear,Const(size.mid.pos.y) > {actionHeightLow})))
Triggerall = !(Ceil(Helper(97530),FVar(39)) & (2**1 + 2**2 + 2**4))
Triggerall = Helper(97530),FVar(37) >= {actionJuggle}
Triggerall = (((P2bodydist X-EnemyNear,Vel X*{actionTime} = [0,({actionFrontDist}-8)+(Stateno = [100,101])*5]) && P2dist X >= 0) || (P2bodydist X < ({actionFrontDist}-3)+(Stateno = [100,101])*5 && P2dist X >= 0))
Triggerall = (EnemyNear,Statetype != A
Triggerall = EnemyNear,Statetype != L && EnemyNear,Movetype = I
Triggerall = EnemyNear,Numproj = 0
Triggerall = +++ || ((EnemyNear,Pos Y + EnemyNear,Vel Y * {actionTime} + EnemyNear,const(movement.yaccel) * {actionTime} * ({actionTime} + 1) / 2 < 0) && EnemyNear,Statetype = A) ;(Helper(97530),FVar(31) = []) 
Triggerall = (ctrl || (Stateno = [19,21]) || (Stateno = [100,101]))
