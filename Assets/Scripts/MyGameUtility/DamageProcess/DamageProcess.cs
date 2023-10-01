using System;

namespace MyGameUtility {
    public static class DamageProcess{
        // public static void CalculateDamage(AttackData attackUnit, DefData defUnit) {
        //     DamageValue unit = new DamageValue();
        //
        //     unit.InitData(attackUnit, defUnit);
        //     
        //     attackUnit.OnApplyDamageBefore?.Invoke(unit);
        //     defUnit.OnApplyDamageBefore?.Invoke(unit);
        //     
        //     defUnit.ApplyDamage(unit);
        //     
        //     attackUnit.OnApplyDamageAfter?.Invoke(unit);
        //     defUnit.OnApplyDamageAfter?.Invoke(unit);
        //
        //     if (defUnit.IsDeath) {
        //         attackUnit.OnKillBefore?.Invoke(unit);
        //         defUnit.OnBeKilledBefore?.Invoke(unit);
        //
        //         if (defUnit.IsDeath) {
        //             attackUnit.OnKill?.Invoke(unit);
        //             defUnit.OnBeKilled?.Invoke(unit);
        //         }
        //     }
        //     
        //     attackUnit.OnDamageProcessAfter?.Invoke(unit);
        //     defUnit.OnDamageProcessAfter?.Invoke(unit);
        // }
        //
        // public static void CalculateDamage(DamageValue unit, IDefData defUnit) {
        //     defUnit.OnApplyDamageBefore?.Invoke(unit);
        //     defUnit.ApplyDamage(unit);
        //     
        //     defUnit.OnApplyDamageAfter?.Invoke(unit);
        //
        //     if (defUnit.IsDeath) {
        //         defUnit.OnBeKilledBefore?.Invoke(unit);
        //
        //         if (defUnit.IsDeath) {
        //             defUnit.OnBeKilled?.Invoke(unit);
        //         }
        //     }
        //     
        //     defUnit.OnDamageProcessAfter?.Invoke(unit);
        // }
        
        public static void CalculateDamage<T, TAtk, TDef, ThpInfo>(TAtk attackUnit, TDef defUnit) 
            where TAtk : BaseAtkData<T, ThpInfo> 
            where TDef  : BaseDefData<T, ThpInfo>
where ThpInfo : BaseHealthGroupInfo<T>
where T : IComparable<T> {

            DamageValue<T> unit = attackUnit.InitData(defUnit.HealthGroupInfo);
            
            attackUnit.OnApplyDamageBefore?.Invoke(unit);
            defUnit.OnApplyDamageBefore?.Invoke(unit);
            
            defUnit.ApplyDamage(unit);
            
            attackUnit.OnApplyDamageAfter?.Invoke(unit);
            defUnit.OnApplyDamageAfter?.Invoke(unit);

            if (defUnit.IsDeath) {
                attackUnit.OnKillBefore?.Invoke(unit);
                defUnit.OnBeKilledBefore?.Invoke(unit);

                if (defUnit.IsDeath) {
                    attackUnit.OnKill?.Invoke(unit);
                    defUnit.OnBeKilled?.Invoke(unit);
                }
            }
            
            attackUnit.OnDamageProcessAfter?.Invoke(unit);
            defUnit.OnDamageProcessAfter?.Invoke(unit);
        }
        
        public static void CalculateDamage<T,ThpInfo>(DamageValue<T> unit, BaseDefData<T, ThpInfo> defUnit) 
            where T : IComparable<T> 
            where  ThpInfo : BaseHealthGroupInfo<T> {
            
            defUnit.OnApplyDamageBefore?.Invoke(unit);
            defUnit.ApplyDamage(unit);
            
            defUnit.OnApplyDamageAfter?.Invoke(unit);
        
            if (defUnit.IsDeath) {
                defUnit.OnBeKilledBefore?.Invoke(unit);
        
                if (defUnit.IsDeath) {
                    defUnit.OnBeKilled?.Invoke(unit);
                }
            }
            
            defUnit.OnDamageProcessAfter?.Invoke(unit);
        }
    }
}