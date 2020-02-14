using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLineLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform FishingLineTip;
    [SerializeField] GameObject fishingBob;


    [SerializeField] int accuracyItirations = 5;


    [SerializeField] Color normal_colour;
    [SerializeField] Color snapping_colour;

    float normal_line_length = 0;
    public enum STATE
    {
        NOT_ACTIVE,
        CASTING,
        FISHING,
        FIGHTING
    };
    STATE current_state = STATE.NOT_ACTIVE;

    bool willReelInOnUpdate = false;

    FishLogic fightingFish;


    void Start()
    {
        LineParticles.Clear();

        LineParticle new_particle = new LineParticle();
        new_particle.position = FishingLineTip.position;
        new_particle.oldPosition = FishingLineTip.position;

        LineParticle new_particle2 = new LineParticle();
        new_particle2.position = FishingLineTip.position;
        new_particle2.oldPosition = FishingLineTip.position;

        LineParticles.AddFirst(new_particle);
        LineParticles.AddFirst(new_particle2);

        GetComponent<LineRenderer>().positionCount = 2;
    }



    float test = 0;
    float setDistanceBetweenParticle0And1 = 0.001f;
    public void CastLine()
    {
        current_state = STATE.CASTING;
        test = 0;
        //for (int i = 0; i < LineParticles.Count; i++)
        //{
        //    LineParticles[i].position = FishingLineTip.position;
        //    LineParticles[i].oldPosition = FishingLineTip.position;
        //}
    }


    float fishing_constraint_distance = 0.0f;
    class LineParticle
    {
       
        public Vector3 oldPosition = Vector3.zero;
        public Vector3 position = Vector3.zero;
        public Vector3 accelleration = Physics.gravity * 0.01f;
        public float dragPercentage = 0.01f;
    }

    LinkedList<LineParticle> LineParticles = new LinkedList<LineParticle>();

    private void OnEnable()
    {
        GetComponent<LineRenderer>().material.color = normal_colour;

        LineParticles.Clear();

        LineParticle new_particle = new LineParticle();
        new_particle.position = FishingLineTip.position;
        new_particle.oldPosition = FishingLineTip.position;

        LineParticle new_particle2 = new LineParticle();
        new_particle2.position = FishingLineTip.position;
        new_particle2.oldPosition = FishingLineTip.position;

        LineParticles.AddFirst(new_particle);
        LineParticles.AddFirst(new_particle2);

        GetComponent<LineRenderer>().positionCount = 2;
    }
    private void OnDisable()
    {
        current_state = STATE.NOT_ACTIVE;
        LineParticles.Clear();
    }
    public void ReelIn()
    {
        willReelInOnUpdate = true;
    }
    public bool IsFullyReeledIn()
    {
        return (LineParticles.Count <= 1 || (willReelInOnUpdate && LineParticles.Count == 2));
    }

    // Update is called once per frame
    void Update()
    {


        // Collisions
            
            for (LinkedListNode<LineParticle> it = LineParticles.First; it != null; it = it.Next)
            {
                bool in_water = false;

                Collider[] colliders = Physics.OverlapSphere(it.Value.position,0);
                for (int c= 0; c < colliders.Length; c++)
                {
                    if (colliders[c].GetComponent<TagsScript>() != null)
                    {
                        if (colliders[c].GetComponent<TagsScript>().ContainsTheTag(TagsScript.TAGS.WATER))
                        {
                            in_water = true;
                            break;
                        }
                    }
                }
                if (in_water)
                {
                    it.Value.accelleration = Physics.gravity * 0.0f;
                    //it.Value.accelleration.x = ((Mathf.PerlinNoise(it.Value.position.x * 8.0f, Time.time) * 2.0f) - 1.0f) * 5.0f;
                    //it.Value.accelleration.z = ((Mathf.PerlinNoise(it.Value.position.z * 8.0f, Time.time) * 2.0f) - 1.0f) * 5.0f;

                     it.Value.dragPercentage = 0.1f;
                }
                else
                {
                    it.Value.accelleration = Physics.gravity * 0.1f;
                    it.Value.dragPercentage = 0.01f;
                }
            }

            switch (current_state)
            {
            case STATE.CASTING: // do casting in update to keep a smooth look as soaring through the sky
                {

                    LineParticles.First.Value.position = FishingLineTip.position;
                    LineParticles.Last.Value.position = fishingBob.transform.position;
                    LineParticles.Last.Value.oldPosition = fishingBob.transform.position;

                    setDistanceBetweenParticle0And1 = Vector3.Distance(LineParticles.First.Value.position, LineParticles.First.Next.Value.position); // change distance between 
                    if (Vector3.Distance(LineParticles.First.Value.position, LineParticles.First.Next.Value.position) > 0.05f)
                    {
                        setDistanceBetweenParticle0And1 = 0.0001f;
                        LineParticle new_particle = new LineParticle();
                        new_particle.position = FishingLineTip.position;
                        new_particle.oldPosition = FishingLineTip.position;
                        LineParticles.AddFirst(new_particle);
                        GetComponent<LineRenderer>().positionCount = LineParticles.Count;
                    }

                    for (LinkedListNode<LineParticle> it = LineParticles.First; it != null; it = it.Next)
                    {
                        Verlet(it.Value);
                    }



                    for (int k = 0; k < accuracyItirations; k++)
                    {
                        LineParticles.First.Value.position = FishingLineTip.position;
                        LineParticles.Last.Value.position = fishingBob.transform.position;
                        PoleConstraint(LineParticles.First.Value, LineParticles.First.Next.Value, setDistanceBetweenParticle0And1); // makes a distance transition between particle 0 and 1

                        for (LinkedListNode<LineParticle> it = LineParticles.Last; it.Previous != LineParticles.First; it = it.Previous)
                        {

                            PoleConstraint(it.Value, it.Previous.Value, 0.1f);

                        }
                    }

                    normal_line_length = 0;
                    for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                    {
                        normal_line_length += Vector3.Distance(it.Value.position, it.Next.Value.position);
                    }

                    break;
                }
            case STATE.FISHING:
                    {

                    if (willReelInOnUpdate)
                    {
                        if (LineParticles.Count != 0)
                        {
                            LineParticles.RemoveFirst();
                            GetComponent<LineRenderer>().positionCount = LineParticles.Count;

                        }
                    }


                    LineParticles.First.Value.position = FishingLineTip.position;
               //     LineParticles.Last.Value.oldPosition = LineParticles.First.Value.position;

                    LineParticles.Last.Value.position = fishingBob.transform.position;
                  //  LineParticles.Last.Value.oldPosition = fishingBob.transform.position;





                    for (LinkedListNode<LineParticle> it = LineParticles.First; it != null; it = it.Next)
                    {
                        Verlet(it.Value);
                    }



                    for (int k = 0; k < accuracyItirations; k++)
                    {
                       // PoleConstraint(LineParticles.First.Value, LineParticles.First.Next.Value, setDistanceBetweenParticle0And1); // makes a distance transition between particle 0 and 1

                        for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                        {
                          //  LineParticles.First.Value.position = FishingLineTip.position;
                           // LineParticles.Last.Value.position = fishingBob.transform.position;

                            PoleConstraint(it.Value, it.Next.Value, fishing_constraint_distance);

                        }
                    }

                    normal_line_length = 0;
                    for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                    {
                        normal_line_length += Vector3.Distance(it.Value.position, it.Next.Value.position);
                    }

                    break;
                    }
                case STATE.FIGHTING:
                    {


                    LineParticles.First.Value.position = FishingLineTip.position;
                    LineParticles.Last.Value.position = fishingBob.transform.position;
                    LineParticles.Last.Value.oldPosition = fishingBob.transform.position;




                    for (LinkedListNode<LineParticle> it = LineParticles.First; it != null; it = it.Next)
                    {
                        Verlet(it.Value);
                    }



                    for (int k = 0; k < accuracyItirations; k++)
                    {
                        for (LinkedListNode<LineParticle> it = LineParticles.Last; it.Previous != null; it = it.Previous)
                        {

                            LineParticles.First.Value.position = FishingLineTip.position;
                            LineParticles.Last.Value.position = fishingBob.transform.position;
                            LineParticles.Last.Value.oldPosition = fishingBob.transform.position;
                            PoleConstraint(it.Value, it.Previous.Value, fishing_constraint_distance);

                        }
                    }

                    normal_line_length = 0;
                    for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                    {
                        normal_line_length += Vector3.Distance(it.Value.position, it.Next.Value.position);
                    }

                    while(true)
                    {
                        float line_distance = 0;
                        for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                        {
                            line_distance += Vector3.Distance(it.Value.position, it.Next.Value.position);
                        }

                        if (line_distance > (Vector3.Distance(fishingBob.transform.position, FishingLineTip.position) - 0.01f))
                        {
                            LineParticles.RemoveLast();
                            GetComponent<LineRenderer>().positionCount = LineParticles.Count;
                            
                        }
                        else
                        {
                            break;
                        }
                    }


                    GetComponent<LineRenderer>().material.color = Color.Lerp(snapping_colour, normal_colour, fightingFish.GetLineStrengthPercentageLeft());

                    break;
                    }


            }

        Vector3[] linePositions = new Vector3[LineParticles.Count];
        int index = 0;
        for (LinkedListNode<LineParticle> it = LineParticles.First; it != null; it = it.Next)
        {

            linePositions[index] =  it.Value.position;
            index++;
        }
        linePositions[0] = FishingLineTip.position;
        linePositions[LineParticles.Count - 1] = fishingBob.transform.position;
        GetComponent<LineRenderer>().SetPositions(linePositions);

        willReelInOnUpdate = false;
    }




    // -- Write Functions -- //
    public void BeganFishing()
    {
        current_state = STATE.FISHING;


        normal_line_length = 0;

        for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
        {
            normal_line_length += Vector3.Distance(it.Value.position, it.Next.Value.position);
        }
        //normal_line_length += Vector3.Distance(FishingLineTip.position, LineParticles.First.Value.position );
        //normal_line_length += Vector3.Distance(LineParticles.Last.Value.position, fishingBob.transform.position);

        fishing_constraint_distance = (normal_line_length / LineParticles.Count) * 0.9f;
    }

    public void BeganFighting(FishLogic fishLogic)
    {
        current_state = STATE.FIGHTING;
        fightingFish = fishLogic;
    }
    // -- Read Functions -- //
    public Vector3 GetEndOfLine()
    {
        return LineParticles.Last.Value.position;
    }

    public Vector3 EndOfLineVelocity()
    {
        return (LineParticles.Last.Value.position - LineParticles.Last.Value.oldPosition);
    }
    public float DistanceFromTipToBob()
    {
        return (Vector3.Distance(FishingLineTip.position, fishingBob.transform.position));
    }
    public Vector3 EndOfLineForce()
    {
        if (current_state != STATE.NOT_ACTIVE)
        {
            float current_rope_length = 0;
            for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
            {
                current_rope_length += Vector3.Distance(it.Value.position, it.Next.Value.position);
            }
            current_rope_length += Vector3.Distance(FishingLineTip.position, LineParticles.First.Value.position);
            current_rope_length += Vector3.Distance(LineParticles.Last.Value.position, fishingBob.transform.position);

            float extra_length = Mathf.Max(current_rope_length - normal_line_length, 0);


            switch(current_state)
            {
                case STATE.CASTING:
                    {
                        return (FishingLineTip.position - fishingBob.transform.position).normalized * extra_length * 5.0f;
                    }
                case STATE.FISHING:
                    {
                        test += Time.deltaTime;
                        return (FishingLineTip.position - fishingBob.transform.position).normalized * extra_length * Mathf.Lerp(3.0f,0.75f,test); // lerp force multiple settles the fishing rod rather than making a clear force difference
                    }
                case STATE.FIGHTING:
                    {
                        return (FishingLineTip.position - fishingBob.transform.position).normalized * extra_length * 10.0f;

                    }
            }

        }


        return Vector3.zero;
    }
    private void Verlet(LineParticle p)
    {
        var temp = p.position;
        p.position += ((p.position - p.oldPosition)*(1.0f - p.dragPercentage)) + (p.accelleration * Time.deltaTime * Time.deltaTime);
        p.oldPosition = temp;


        // default Verlet
        // particle.position +=  (particle.position - particle.oldPosition) + (particle.accelleration * Time.deltaTime * Time.deltaTime);
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
