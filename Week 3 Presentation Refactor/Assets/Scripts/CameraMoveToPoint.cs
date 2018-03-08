using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	/// <summary>
	/// Camera movement
	/// </summary>
    public class CameraMoveToPoint : MonoBehaviour
    {
        
        public Vector3 targetPosition; //position to go
        public Vector3 startPostion; //start position (transform.position)
        [SerializeField] private float duration = 25f; //one way duration

        private bool isAtGoal = false;
        IEnumerator Start ()
        {
            //Set the start position
            startPostion = this.transform.position;

            //While playing the scene go to target position and back
            while (!isAtGoal)
            {
                yield return StartCoroutine (MoveCamera (startPostion, targetPosition));
               // yield return StartCoroutine (MoveCamera (targePosition, startPostion));
            }
        }

        /// <summary>
        /// Move camera from start point to target point based on duration, using lerp
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        IEnumerator MoveCamera (Vector3 startPoint, Vector3 targetPoint)
        {
            //Initialize the function point and the rate based on duration
            float i = 0f;
            float rate = 1 / duration;

            while (i < 1f)
            {
                //Lerp the position
                i += Time.deltaTime * rate;
                this.transform.position = Vector3.Lerp (startPoint, targetPoint, i);
                if (this.transform.position.Equals(targetPoint)) {
                    isAtGoal = true;
                }
                yield return null;
            }
        }

        /// <summary>
        /// Draw the camera path
        /// </summary>
        void OnDrawGizmosSelected ()
        {
            //Draw the camera path
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere (this.transform.position, 0.3f);
            Gizmos.DrawLine (this.transform.position, targetPosition);
            Gizmos.DrawWireSphere (targetPosition, 0.3f);
        }
    }
