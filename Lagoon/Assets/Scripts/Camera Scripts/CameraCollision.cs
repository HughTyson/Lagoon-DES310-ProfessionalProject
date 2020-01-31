using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    // ==========================================
    //              Visible Variables
    //===========================================

    [SerializeField] LayerMask camera_collisison_layer;

    
    //collision detection variables 

    [SerializeField] public bool collision = false;

    [SerializeField] public Vector3[] adjustedCameraClipPoints;
    [SerializeField] public Vector3[] desiredCameraClipPoints;

    [Range(2.0f, 4.0f)]
    [SerializeField] float collision_box_size = 3.41f;

    // ==========================================
    //              Hidden Variables
    //===========================================

    Camera camera;

    public void Initialize(Camera cam)
    {
        camera = cam;
        adjustedCameraClipPoints = new Vector3[5]; //4 clip points and the cameras position
        desiredCameraClipPoints = new Vector3[5];

        camera_collisison_layer.value = 9;

        Debug.Log(camera_collisison_layer.value);
    }

    public void UpdateCameraClipPoints(Vector3 camera_position, Quaternion cp_rotation, ref Vector3[] intoArray)
    {
        //clear intoArray 

        intoArray = new Vector3[5];

        float z = camera.nearClipPlane; //distance from cameras position to the new clip plane
        float x = Mathf.Tan(camera.fieldOfView / collision_box_size) * z;
        float y = x / camera.aspect;

        //top left
        intoArray[0] = (cp_rotation * new Vector3(-x, y, z)) + camera_position;     //add and rotate the collision point based on camera top right
                                                                                
        intoArray[1] = (cp_rotation * new Vector3(x, y, z)) + camera_position;      //add and rotate the collision point based on camera bottom left
                                                                                    
        intoArray[2] = (cp_rotation * new Vector3(-x, -y, z)) + camera_position;    //add and rotate the collision point based on camera bottom right
                                                                                    
        intoArray[3] = (cp_rotation * new Vector3(x, -y, z)) + camera_position;     //add and rotate the collision point based on camera
                                                                                
        intoArray[4] = camera_position - camera.transform.forward/2;                  //cam_pos
    }

    //determines if there is a collision at any of these clip points
    bool CollisionDetectedAtClipPoints(Vector3[] clip_points, Vector3 target_position)
    {
        
        for (int i = 0; i < clip_points.Length; i++)
        {
            Ray ray = new Ray(target_position, clip_points[i] - target_position); //cast ray at targets poisiton for the distance between clip point and target
            float distance = Vector3.Distance(clip_points[i], target_position); //set distance that the ray will be 
            RaycastHit hit;

            //Physics.Raycast(clip_points[i], Vector3.Distance(clip_points[i], target_position), hit, distance)

            Debug.Log(camera_collisison_layer.value);
            
            if(Physics.Raycast(ray, out hit, camera_collisison_layer))
            {
                return true;
            }

            //if (Physics.Raycast(ray, out hit)) //if collision within this ray
            //{
            //    return true;
            //}
        }

        return false;
    }

    public float GetAdjustedDistanceWithRay(Vector3 target_position)
    {
        float distance = -1;

        for (int i = 0; i < desiredCameraClipPoints.Length; i++)
        {
            Ray ray = new Ray(target_position, desiredCameraClipPoints[i] - target_position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (distance == -1) //chekc if any hit has occured yet
                {
                    distance = hit.distance; //if no hit then set as initital hit
                }
                else
                {
                    if (hit.distance < distance) //get the shortest distance from a point to the targets position
                    {
                        distance = hit.distance;
                    }
                }
            }
        }

        if (distance == -1) // if distacne is still negative one then no collisisons;
        {
            return 0;
        }
        else
        {
            return distance;
        }
    }

    public void CheckColliding(Vector3 target_position)
    {

        if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, target_position))
        {
            collision = true;
        }
        else
        {
            collision = false;
        }

    }

}
