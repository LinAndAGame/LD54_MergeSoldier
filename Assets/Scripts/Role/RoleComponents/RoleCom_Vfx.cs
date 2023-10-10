using DG.Tweening;
using Effects;
using MyGameUtility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace Role {
    public class RoleCom_Vfx : BaseRoleComponent<BaseRoleCtrl> {
        public float          HitEffectDuration = 0.1f;
        public string         HitEffectName;
        public float          DeathEffectDuration = 0.5f;
        public string         DeathEffectName;
        public string         LevelUpEffectName = "OUTBASE_ON";
        public SpriteRenderer SR_Self;
        public AssetReference   AssetRef_DamageEffect;

        private Tweener _CurEffectTweener;

        public override void Init() {
            base.Init();
            SR_Self.material.SetFloat(HitEffectName, 0);
            SR_Self.material.SetFloat(DeathEffectName, 0);
        }

        public void PlayBeHitEffect() {
            _CurEffectTweener?.Kill(true);
            _CurEffectTweener = SR_Self.material.DOFloat(1, HitEffectName, HitEffectDuration).SetLoops(2,LoopType.Yoyo);
        }

        public override void DoOnDeath() {
            _CurEffectTweener?.Kill(true);
            _CurEffectTweener = SR_Self.material.DOFloat(1, DeathEffectName, DeathEffectDuration);
        }

        public void PlayLevelUpEffect() {
            SR_Self.material.EnableKeyword(LevelUpEffectName);
        }

        public void CreateDamageEffect(float damage, Vector3 pos) {
            var ins = MyPoolSimpleComponent.Get<TextEffect>(AssetRef_DamageEffect);
            ins.PlayEffect(damage.ToString(), pos);
        }
    }
}