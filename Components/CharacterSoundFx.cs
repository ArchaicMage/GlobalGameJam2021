using UnityEngine;

namespace Com.BigSweater.Components
{
    public class CharacterSoundFx : MonoBehaviour
    {
        [SerializeField] private AudioClip _footstep;
        
        [SerializeField] private AudioClip _jumpStart;
        
        [SerializeField] private AudioClip _jumpLand;
        
        public AudioSource Footsteps => _footsteps;
        
        public AudioSource Jump => _jump;
        
        [SerializeField] private AudioSource _footsteps;
        
        [SerializeField] private AudioSource _jump;

        public void PlayFootstep()
        {
            PlaySoundFx(_footsteps, _footstep);
        }
        
        public void PlayJump()
        {
            PlaySoundFx(_jump, _jumpStart);
        }
        
        public void PlayLand()
        {
            PlaySoundFx(_jump, _jumpLand);
        }

        private void PlaySoundFx(AudioSource source, AudioClip clip)
        {
            if (source.isPlaying)
                source.Stop();

            if (ReferenceEquals(null, clip))
                return;
            
            source.clip = clip;
            source.Play();
        }
    }
}