using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMan : MonoBehaviour
{
    public enum ZTrackingStrategy
    {
        /// <summary>
        /// CameraMan uses the z-offset from OffsetVectorToTrackedTransform as the exact distance
        /// between himself and the tracked target.
        /// </summary>
        Exact,

        /// <summary>
        /// CamearMan won't move from his initial Z coordinate
        /// </summary>
        Frozen,

        /// <summary>
        /// CameraMan will follow tracked object if it leaves the deadzone.
        /// Will also move Camera between LowZTransform and HighZTransform position, depending on where object is in the dead zone.
        /// </summary>
        Lerp,

        /// <summary>
        /// // CameraMan will follow tracked object if it leaves the dead zone
        /// </summary>
        Normal
    }

    public Camera MyCamera;
    public Camera ProjectedCamera;

    public bool EnableInEditMode = false;
    public bool EnableTransformTracking = true;
    public bool IsCinematic = false;
    public Transform TransformToTrack;

    public Transform DefaultTransform;

    public ZTrackingStrategy ZTracking = ZTrackingStrategy.Exact;
    public Transform LowZTransform;
    public Transform HighZTransform;
    public bool smoothMovement = true;
    public Bounds DeadZone = new Bounds();

    public Bounds CameraBounds;

    public Vector3 OffsetVectorToTrackedTransform;

    public Vector3 TargetPosition;
    public Quaternion TargetRotation;

    public Vector3 TargetCameraPosition;
    public Quaternion TargetCameraRotation;

    public float TranslatationLerpFactor = .5f;
    public float RotationLerpFactor = .5f;

    public static CameraMan instance;

    public float CameraMoveSpeed = 10.0f;
    public float CameraTurnSpeed = 10.0f;

    public void Awake()
    {
        if (CameraMan.instance == null)
        {
            instance = this;
        }
        else if (this != instance)
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

        //if (TransformToTrack != null)
        //{
        //    OffsetVectorToTrackedTransform = TransformToTrack.position - this.transform.position;
        //}
    }

    public void StartCinematicMode(Transform cameraPosition)
    {
        StartCinematicMode(cameraPosition.position, cameraPosition.rotation);
    }

    public void StartCinematicMode(Vector3 position, Quaternion rotation)
    {
        IsCinematic = true;
        TargetPosition = position;
        TargetRotation = rotation;

        //make sure the projected Camera is always in the final desired position for accurate worldToScreen tracking
        ProjectedCamera.transform.position = TargetPosition;
        ProjectedCamera.transform.rotation = TargetRotation;
    }

    public void EndCinematicMode()
    {
        IsCinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        //make sure the projected Camera is always in the final desired position for accurate worldToScreen tracking
        ProjectedCamera.transform.position = TargetPosition;
        ProjectedCamera.transform.rotation = TargetRotation;

        // Allow camera logic in Editor if EnableInEditMode is true
        if (!Application.isPlaying && !EnableInEditMode)
        {
            return;
        }

        if (IsCinematic)
        {
            // This effectively means the CameraMan gives up the Camera.
            MyCamera.transform.position = Vector3.Lerp(MyCamera.transform.position, TargetPosition, TranslatationLerpFactor);
            MyCamera.transform.rotation = Quaternion.Lerp(MyCamera.transform.rotation, TargetRotation, RotationLerpFactor);
        }
        else if (EnableTransformTracking)
        {
            if (TransformToTrack == null)
            {
                Debug.LogWarning("CameraMan has transform tracking enabled, but no transform was provided.");
                return;
            }

            DeadZone.center = OffsetVectorToTrackedTransform + this.transform.position;
            if (!DeadZone.Contains(TransformToTrack.position))
            {
                Vector3 closestPoint = DeadZone.ClosestPoint(TransformToTrack.position);
                //Debug.Log($"Closest Point: {closestPoint}");
                Vector3 moveAmount = TransformToTrack.position - closestPoint;
                //Debug.Log($"Camera moveAmount: {moveAmount}");
                TargetPosition = this.transform.position + moveAmount;
            }
            else
            {
                TargetPosition = this.transform.position;
            }

            TargetRotation = this.transform.rotation;

            // By default, MyCamera is not expected to move.
            TargetCameraPosition = MyCamera.transform.position;
            TargetCameraRotation = MyCamera.transform.rotation;
            if (ZTracking == ZTrackingStrategy.Frozen)
            {
                // CameraMan stays locked to the original XY-plane (z-coordinate)
                TargetPosition.z = this.transform.position.z; 
            }
            else if (ZTracking == ZTrackingStrategy.Lerp)
            {
                // CameraMan stays locked to his the XY-plane
                //TargetPosition.z = TransformToTrack.position.z - OffsetVectorToTrackedTransform.z;

                float minZ = DeadZone.min.z;
                float maxZ = DeadZone.max.z;
                float zPoint = (TransformToTrack.position.z - minZ) / (maxZ - minZ);
                TargetCameraPosition = Vector3.Lerp(LowZTransform.position, HighZTransform.position, zPoint);
                TargetCameraRotation = Quaternion.Lerp(LowZTransform.rotation, HighZTransform.rotation, zPoint);

                MyCamera.transform.position = Vector3.Lerp(MyCamera.transform.position, TargetCameraPosition, TranslatationLerpFactor);
                MyCamera.transform.rotation = Quaternion.Lerp(MyCamera.transform.rotation, TargetCameraRotation, RotationLerpFactor);
            }
            else if (ZTracking == ZTrackingStrategy.Exact)
            {
                TargetPosition.z = TransformToTrack.position.z - OffsetVectorToTrackedTransform.z;
            }
            else if (ZTracking == ZTrackingStrategy.Normal)
            {
                // Do nothing. This is taken care of by the dead zone code above
            }

            if(CameraBounds != null)
                TargetPosition = CameraBounds.ClosestPoint(TargetPosition);

            //MyCamera.transform.position = Vector3.Lerp(MyCamera.transform.position, TargetPosition, TranslatationLerpFactor);
            //MyCamera.transform.rotation = Quaternion.Lerp(MyCamera.transform.rotation, TargetRotation, RotationLerpFactor);
            if(smoothMovement)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPosition, CameraMoveSpeed * Time.deltaTime);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, TargetRotation, CameraTurnSpeed * Time.deltaTime);
            }
            else
            {
                this.transform.position = TargetPosition;
                this.transform.rotation = TargetRotation;            
            }
        }
        else
        {
            // Do nothing if no other mode is enabled.
        }
    }
    
    //Used to wait for the camera to get into position so that we can determine where to put the speech bubble
    //This approach is flawed as we should ideally know where the speech bubbles should go given the position of the camera and the actors. 
    //In the future we should be able to do this check without having to wait for the camera to get into position
    public bool InDesiredPosition()
    {
        if (IsCinematic)
        {
            // In Cinematic mode, TargetPosition is the target for MyCamera.
            return (MyCamera.transform.position - TargetPosition).sqrMagnitude < 0.07f * 0.07f
                && Quaternion.Dot(MyCamera.transform.rotation, TargetRotation) > 0.99939; // 2 degrees
        }
        else
        {
            // In non-Cinematic mode, TargetPosition is the location of the CameraMan.
            return (transform.position - TargetPosition).sqrMagnitude < 0.07f * 0.07f
                && Quaternion.Dot(transform.rotation, TargetRotation) > 0.99939 // 2 degrees
                && (MyCamera.transform.position - TargetCameraPosition).sqrMagnitude < 0.07f * 0.07f
                && Quaternion.Dot(MyCamera.transform.rotation, TargetCameraRotation) > 0.99939; // 2 degrees
        }
    }

    public void MoveCameraToDefault()
    {
        MyCamera.transform.CopyValues(DefaultTransform);
    }

    public void MoveCameraToHighZ()
    {
        MyCamera.transform.CopyValues(HighZTransform);
    }

    public void MoveCameraToLowZ()
    {
        MyCamera.transform.CopyValues(LowZTransform);
    }

    public void SaveCameraValuesToDefault()
    {
        DefaultTransform.CopyValues(MyCamera.transform);
    }

    public void SaveCameraValuesToHighZ()
    {
        HighZTransform.CopyValues(MyCamera.transform);
    }

    public void SaveCameraValuesToLowZ()
    {
        LowZTransform.CopyValues(MyCamera.transform);
    }

    public void SaveCameraValuesToNewTransform()
    {
        GameObject obj = new GameObject("Saved Camera Position");
        obj.transform.SetParent(this.transform);
        obj.transform.CopyValues(MyCamera.transform);
    }
}
