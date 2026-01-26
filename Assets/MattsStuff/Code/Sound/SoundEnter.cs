using UnityEngine;

namespace MattsSound.SoundManager
{
    public class PlaySoundEnter : StateMachineBehaviour
    {
        [SerializeField] private SoundType sound;
        [SerializeField, Range(0, 1)] private float volume = 1;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Debug.Log("Playing sound: " + sound);
            SoundManager.PlaySound(sound, null, volume);
        }
    }
}
