using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    // ==========================================
    //              Visible Variables
    //===========================================

    int camera_collisison_layer;

    
    //collision detection variables 

    [SerializeField] public bool collision = false;

    [SerializeField] public Vector3[] adjusted_cp_pos;
    [SerializeField] public Vector3[] desired_cp_pos;

    [Range(0.1f, 4.0f)]
    [SerializeField] float collision_box_size = 3.41f;

    // ==========================================
    //              Hidden Variables
    //===========================================

    Camera camera;

    int layer1 = 9;int layer2 = 10; int layer3 = 11; int layer4 = 12; int layer5 = 13;

    public void Initialize(Camera cam)
    {
        camera = cam;
        adjusted_cp_pos = new Vector3[5]; //4 clip points and the cameras position
        desired_cp_pos = new Vector3[5];

        camera_collisison_layer = (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13);

        camera_collisison_layer = ~camera_collisison_layer;


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
            
            if(Physics.Raycast(ray, out hit, distance, camera_collisison_layer))
            {
                return true;
            }
        }

        return false;
    }

    public float GetAdjustedDistanceWithRay(Vector3 target_position)
    {
        float distance = -1;

        for (int i = 0; i < desired_cp_pos.Length; i++)
        {
            Ray ray = new Ray(target_position, desired_cp_pos[i] - target_position);
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

        if (CollisionDetectedAtClipPoints(desired_cp_pos, target_position))
        {
            collision = true;
        }
        else
        {
            collision = false;
        }

    }

}
