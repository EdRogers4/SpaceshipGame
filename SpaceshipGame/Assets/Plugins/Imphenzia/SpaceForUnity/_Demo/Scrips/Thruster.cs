// Thruster C# Script (version: 1.6)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2019 Imphenzia AB

// DESCRIPTION:
// This script controls the thruster used for spaceship propulsion.

// INSTRUCTIONS:
// Use the thruster prefab which has the required particle system this script depends upon.
// Configure the thruster force and other parameters as desired.

// Version History
// 1.6  - New Imphenzia.SpaceForUnity namespace to replace SU_ prefix.
//      - Moved asset into Plugins/Imphenzia/SpaceForUnity for asset best practices.
// 1.06 - Updated for Unity 5.5, removed deprecated code. Changed light cache from transform to component.
// 1.02 - Prefixed with SU_Thruster to avoid naming conflicts.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

namespace Imphenzia.SpaceForUnity
{
    public class Thruster : MonoBehaviour
    {
        [Tooltip("The thruster force to be applied to it's rigidbody parent when active")]
        public float thrusterForce = 10000;

        [Tooltip("Whether or not to add force at position which introduces torque, use with care as it is super sensitive for positioning")]
        public bool addForceAtPosition = false;
        
        [Tooltip("Sound effect volume of thruster")]
        public float soundEffectVolume = 1.0f;

        // Private variables
        private bool _isActive = false;
        private Transform _cacheTransform;
        private Rigidbody _cacheParentRigidbody;
        private Light _cacheLight;
        private ParticleSystem _cacheParticleSystem;

        /// <summary>
        /// Call StartThruster() function from other scripts to start the thruster 
        /// </summary>
        public void StartThruster()
        {
            // Set the thruster active flag to true
            _isActive = true;
        }

        /// <summary>
        /// Call StopThruster() function from other scripts to stop the thruster
        /// </summary>
        public void StopThruster()
        {
            // Set the thruster active flag to false		
            _isActive = false;
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        void Start()
        {
            // Cache the transform and parent rigidbody to improve performance
            _cacheTransform = transform;

            // Ensure that parent object (e.g. spaceship) has a rigidbody component so it can apply force.
            if (transform.parent.GetComponent<Rigidbody>() != null)
            {
                _cacheParentRigidbody = transform.parent.GetComponent<Rigidbody>();
            }
            else
            {
                Debug.LogError("Thruster has no parent with rigidbody that it can apply the force to.");
            }

            // Cache the light source to improve performance (also ensure that the light source in the prefab is intact.)
            _cacheLight = transform.GetComponent<Light>().GetComponent<Light>();
            if (_cacheLight == null)
            {
                Debug.LogError("Thruster prefab has lost its child light. Recreate the thruster using the original prefab.");
            }
            // Cache the particle system to improve performance (also ensure that the particle system in the rpefab is intact.)
            _cacheParticleSystem = GetComponent<ParticleSystem>();
            if (_cacheParticleSystem == null)
            {
                Debug.LogError("Thruster has no particle system. Recreate the thruster using the original prefab.");
            }

            // Start the audio loop playing but mute it. This is to avoid play/stop clicks and clitches that Unity may produce.
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().volume = soundEffectVolume;
            GetComponent<AudioSource>().mute = true;
            GetComponent<AudioSource>().Play();
        }

        /// <summary>
        /// Runs every frame.
        /// </summary>
        void Update()
        {
            // If the light source of the thruster is intact...
            if (_cacheLight != null)
            {
                // Set the intensity based on the number of particles
                _cacheLight.intensity = _cacheParticleSystem.particleCount / 20;
            }

            // If the thruster is active...
            if (_isActive)
            {
                // ...and if audio is muted...
                if (GetComponent<AudioSource>().mute)
                {
                    // Unmute the audio
                    GetComponent<AudioSource>().mute = false;
                }
                // If the audio volume is lower than the sound effect volume...
                if (GetComponent<AudioSource>().volume < soundEffectVolume)
                {
                    // ...fade in the sound (to avoid clicks if just played straight away)
                    GetComponent<AudioSource>().volume += 5f * Time.deltaTime;
                }

                // If the particle system is intact...
                if (_cacheParticleSystem != null)
                {
                    // Enable emission of thruster particles
                    ParticleSystem.EmissionModule _em = _cacheParticleSystem.emission;
                    _em.enabled = true;
                }
            }
            else
            {
                // The thruster is not active
                if (GetComponent<AudioSource>().volume > 0.01f)
                {
                    // ...fade out volume
                    GetComponent<AudioSource>().volume -= 5f * Time.deltaTime;
                }
                else
                {
                    // ...and mute it when it has faded out
                    GetComponent<AudioSource>().mute = true;
                }

                // If the particle system is intact...
                if (_cacheParticleSystem != null)
                {
                    // Stop emission of thruster particles
                    ParticleSystem.EmissionModule _em = _cacheParticleSystem.emission;
                    _em.enabled = false;
                }

            }

        }

        /// <summary>
        /// Runs ever physics update.
        /// </summary>
        void FixedUpdate()
        {
            // If the thruster is active...
            if (_isActive)
            {
                // ...add the relative thruster force to the parent object
                if (addForceAtPosition)
                {
                    // Add force relative to the position on the parent object which will also apply rotational torque
                    _cacheParentRigidbody.AddForceAtPosition(_cacheTransform.up * thrusterForce, _cacheTransform.position);
                }
                else
                {
                    // Add force without rotational torque
                    _cacheParentRigidbody.AddRelativeForce(Vector3.forward * thrusterForce);
                }
            }
        }
    }
}