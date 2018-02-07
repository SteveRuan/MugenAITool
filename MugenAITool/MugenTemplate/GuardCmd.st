
;---------------------------------------------------------------------------
[State -1, 地面防御]
type = ChangeState
value = 120
Triggerall = !Ishelper && EnemyNear,Alive
Triggerall = Var(59) > 0
Triggerall = Roundstate = 2
Triggerall = Statetype != A
Triggerall = (ctrl || (Stateno = [19,21])) && (Stateno != [120,140))
Triggerall = !(EnemyNear,hitdefattr = sca,at)
Triggerall = !(Ceil(Helper(97530),FVar(39)) & 2**0) 
Triggerall = (Helper(97530),FVar(32) < 1000 - (((6-Var(59))**2) - 1) * 25) || (EnemyNear,Time > ((6-Var(59))**2)) || Var(59) = 5
Trigger1 = Inguarddist || (!InGuardDist && EnemyNear,P2dist X < 5 && P2bodydist X < 60 && EnemyNear,Statetype = A && (EnemyNear,HitDefAttr = A,AA || EnemyNear,Movetype = A))
Trigger2 = EnemyNear,Numproj > 0 || (EnemyNear,Numhelper > Helper(97530),FVar(35) && (EnemyNear,Movetype = A || Random > 750))
Trigger2 = Helper(97531),Inguarddist && Helper(97530),FVar(38)/1000 > 0

;---------------------------------------------------------------------------
[State -1, 空中防御]
type = {CanAirGuard}ChangeState
value = 120
Triggerall = !Ishelper && EnemyNear,Alive
Triggerall = Var(59) > 0 
Triggerall = Roundstate = 2
Triggerall = Statetype = A
Triggerall = ctrl
Triggerall = !(EnemyNear,hitdefattr = sca,at)
Triggerall = !(Ceil(Helper(97530),FVar(39)) & 2**0) 
Triggerall = (Helper(97530),FVar(32) < 1000 - (((6-Var(59))**2) - 1) * 25) || (EnemyNear,Time > ((6-Var(59))**2)) || Var(59) = 5
Trigger1 = Inguarddist
Trigger2 = !InGuardDist && EnemyNear,P2dist X < 5 && P2bodydist X < 60 && (EnemyNear,HitDefAttr = SCA,AA || (EnemyNear,Movetype = A && EnemyNear,Statetype = A))

