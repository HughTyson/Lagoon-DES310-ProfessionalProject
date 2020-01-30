using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLineLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform FishingLineTip;
    [SerializeField] float lineLength = 1;
    [SerializeField] int particleAmmount = 10;
    [SerializeField] int accuracyItirations = 5;
    [SerializeField] GameObject fishingRod;

    [SerializeField] float fishingBobMass = 0.5f;
    GameObject fishingBob;


    public enum STATE
    { 
        LOOSE_STRING,
        NORMAL,
        NOT_ACTIVE
    };
    STATE current_state = STATE.NOT_ACTIVE;

    public void SetState(STATE state)
    {
        current_state = state;
    }

    class LineParticle
    {
        public Vector3 oldPosition = Vector3.zero;
        public Vector3 position = Vector3.zero;
        public Vector3 accelleration = Physics.gravity;
        public float dragPercentage = 0.0f;
    }

    List<LineParticle> LineParticles = new List<LineParticle>();

    public void setupLine(GameObject fishingBob_)
    {
        fishingBob = fishingBob_;
        for (int i = 0; i < LineParticles.Count; i++)
        {
            LineParticles[i].position = FishingLineTip.position;
            LineParticles[i].oldPosition = FishingLineTip.position;

        }
    }
    void Start()
    {
        for (int i = 0; i < particleAmmount; i++)
        {
            LineParticles.Add(new LineParticle());
       
        }

      //  LineParticles[0].dragPercentage = 1.0f;
        //LineParticles[1].dragPercentage = 1.0f;

     //   LineParticles[LineParticles.Count - 1].dragPercentage = 1.0f;


        LineParticles[LineParticles.Count - 2].dragPercentage = 0.5f;
    }


    public void LooseString()
    {
        current_state = STATE.LOOSE_STRING;
        for (int i = 0; i < LineParticles.Count; i++)
        {
            LineParticles[i].oldPosition = FishingLineTip.position;
            LineParticles[i].position = FishingLineTip.position;
        }

    }

    public void ReelIn()
    {
        lineLength -= 2.0f * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fishingBob == null)
        {
            current_state = STATE.NOT_ACTIVE;
            GetComponent<LineRenderer>().enabled = false;
        }
        else
        {
            GetComponent<LineRenderer>().enabled = true;

            for (int i = 1; i < LineParticles.Count - 1; i++)
            {
                bool in_water = false;

                Collider[] colliders = Physics.OverlapSphere(LineParticles[i].position,0);
                for (int c= 0; c < colliders.Length; c++)
                {
                    if (colliders[c].GetComponent<TagsScript>() != null)
                    {

                        if (colliders[c].GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.COLLIDES_WITH_ROPE__BOX_COLLIDER))
                        {
                        //    LineParticles[i].position = LineParticles[i].oldPosition; // temporary
                     


                            break;
                        }
                        else if (colliders[c].GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.WATER))
                        {
                            in_water = true;
                            break;
                        }



                    }
                }
                if (in_water)
                {
                    LineParticles[i].accelleration = Physics.gravity * -0.05f;

                    //if (i != 1 && i != LineParticles.Count - 2)
                    //{
                    //    LineParticles[i].dragPercentage = 0.01f;
                    //}
                }
                else
                {
                    LineParticles[i].accelleration = Physics.gravity;

                    //if (i != 1 && i != LineParticles.Count - 2)
                    //{
                    //    LineParticles[i].dragPercentage = 0;
                    //}
                }
            }

            switch (current_state)
            {
                case STATE.LOOSE_STRING:
                    {
                        LineParticles[0].position = FishingLineTip.position;
                        LineParticles[LineParticles.Count - 1].position = fishingBob.transform.position;
                        lineLength = Mathf.Max(0.01f, Vector3.Distance(FishingLineTip.position, fishingBob.transform.position) * 1.1f);

                        for (int i = 1; i < LineParticles.Count; i++)
                        {
                            Verlet(LineParticles[i]);
                        }


                        for (int k = 0; k < accuracyItirations; k++)
                        {
                            for (int i = 0; i < LineParticles.Count - 1; i++)
                            {
                                PoleConstraint(LineParticles[i], LineParticles[i + 1], lineLength / (float)LineParticles.Count);
                            }
                        }

                        Vector3[] linePositions = new Vector3[LineParticles.Count];
                        for (int i = 0; i < LineParticles.Count; i++)
                        {
                            linePositions[i] = LineParticles[i].position;
                        }
                        linePositions[0] = FishingLineTip.position;
                        linePositions[LineParticles.Count - 1] = fishingBob.transform.position;
                        GetComponent<LineRenderer>().SetPositions(linePositions);
                        break;
                    }
                case STATE.NORMAL:
                    {


                        LineParticles[0].position = FishingLineTip.position;
                     //  LineParticles[0].oldPosition = FishingLineTip.position;

                        LineParticles[LineParticles.Count - 1].position = fishingBob.transform.position;
                    //    LineParticles[LineParticles.Count - 1].oldPosition = fishingBob.transform.position;


                        for (int i = 0; i < LineParticles.Count - 1; i++)
                        {
                            Verlet(LineParticles[i]);
                        }

                       

                        for (int k = 0; k < accuracyItirations; k++)
                        {
                            for (int i = 0; i < LineParticles.Count - 1; i++)
                            {
                                PoleConstraint(LineParticles[i], LineParticles[i + 1], lineLength / (float)LineParticles.Count);
                            }
                        }

                        fishingBob.GetComponent<Rigidbody>().AddForce((LineParticles[LineParticles.Count - 1].position - fishingBob.transform.position) / fishingBobMass, ForceMode.VelocityChange);

              

                        //if (lineLength < Vector3.Distance(FishingLineTip.position, fishingBob.transform.position))
                        //{
                        //    float strength = Vector3.Distance(FishingLineTip.position, fishingBob.transform.position) - lineLength;
                        //    float max_dist = 1.0f;
                        //    float max_mag = 10.0f;
                        //    fishingRod.GetComponent<Rigidbody>().AddForceAtPosition(FishingLineTip.position, (fishingBob.transform.position - FishingLineTip.position).normalized * -Mathf.Lerp(0,max_mag,strength/max_dist), ForceMode.Acceleration);
                        //}

                        Vector3[] linePositions = new Vector3[LineParticles.Count];
                        for (int i = 0; i < LineParticles.Count; i++)
                        {
                            linePositions[i] = LineParticles[i].position;
                        }
                        linePositions[0] = FishingLineTip.position;
                        linePositions[LineParticles.Count - 1] = fishingBob.transform.position;
                        GetComponent<LineRenderer>().SetPositions(linePositions);

                        break;
                    }
                case STATE.NOT_ACTIVE:
                    {
                        GetComponent<LineRenderer>().enabled = false;
                        break;
                    }


            }
        }
       



    }

  
    private void Verlet(LineParticle p)
    {
        var temp = p.position;
        p.position += ((p.position - p.oldPosition)*(1.0f - p.dragPercentage)) + (p.accelleration * Time.fixedDeltaTime * Time.fixedDeltaTime);
        p.oldPosition = temp;
    }

    private void PoleConstraint(LineParticle p1, LineParticle p2, float restLength)
    {
        var delta = p2.position - p1.position;

        var deltaLength = delta.magnitude;

        var diff = (deltaLength - restLength) / deltaLength;

        p1.position += delta * diff * 0.5f;
        p2.position -= delta * diff * 0.5f;
    }
}
