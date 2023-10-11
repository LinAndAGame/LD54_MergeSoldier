using System.Collections;
using UnityEngine;

namespace Role {
    public class RoleCom_Animation : BaseRoleComponent<BaseRoleCtrl> {
        public float    DeathDestroyDelay = 0.5f;
        public Animator AnimatorRef;
        public string   DeathAnimationName;
        public string   BeHitAnimationName;
        public string   AttackAnimationName;
        
        

        public void PlayDeathAnimation() {
            PlayAnimation(DeathAnimationName);

            StartCoroutine(delayDestroy());

            IEnumerator delayDestroy() {
                yield return new WaitForSeconds(DeathDestroyDelay);
                Owner.DestroySelf();
            }
        }

        public void PlayBeHitAnimation() {
            PlayAnimation(BeHitAnimationName);
        }

        public void PlayAttackAnimation() {
            PlayAnimation(AttackAnimationName);
        }

        private void PlayAnimation(string triggerName) {
            AnimatorRef.SetTrigger(triggerName);
        }

        public override void DoOnDeath() {
            PlayDeathAnimation();
        }
    }
}