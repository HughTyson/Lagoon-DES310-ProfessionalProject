using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLineLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform FishingLineTip;
    [SerializeField] GameObject fishingBob;


    [SerializeField] int accuracyItirations = 5;


    [SerializeField] float hooksLawLineConstant = 1.0f;

    float normal_line_length = 0;
    public enum STATE
    {
        NOT_ACTIVE,
        CASTING,
        FISHING,
        FIGHTING
    };
    STATE current_state = STATE.NOT_ACTIVE;


    


    void Start()
    {
        //for (int i = 0; i < particleAmmount; i++)
        //{
        //    LineParticles.Add(new LineParticle());
       
        //}

        //LineParticles[LineParticles.Count - 2].dragPercentage = 0.5f;
    }

    
    public void CastLine()
    {
        current_state = STATE.CASTING;
        //for (int i = 0; i < LineParticles.Count; i++)
        //{
        //    LineParticles[i].position = FishingLineTip.position;
        //    LineParticles[i].oldPosition = FishingLineTip.position;
        //}
    }

    class LineParticle
    {
       
        public Vector3 oldPosition = Vector3.zero;
        public Vector3 position = Vector3.zero;
        public Vector3 accelleration = Physics.gravity;
        public float dragPercentage = 0.0f;
    }

    LinkedList<LineParticle> LineParticles = new LinkedList<LineParticle>();

    private void OnEnable()
    {
        //for (int i = 0; i < LineParticles.Count; i++)
        //{
        //    LineParticles[i].position = FishingLineTip.position;
        //    LineParticles[i].oldPosition = FishingLineTip.position;
        //}

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
        if (LineParticles.Count != 0)
        {
            LineParticles.RemoveFirst();
            GetComponent<LineRenderer>().positionCount = LineParticles.Count;


        }
    }
    public bool IsFullyReeledIn()
    {
        return (LineParticles.Count <= 1);
    }

    // Update is called once per frame
    void Update()
    {
        FishingLineTip.GetComponent<FishingRodLogic>().SafetyUpdate();


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
                    it.Value.accelleration = Physics.gravity * -0.2f;

                    it.Value.accelleration.x = ((Mathf.PerlinNoise(it.Value.position.x*8.0f, Time.time) * 2.0f) - 1.0f) * 5.0f;
                    it.Value.accelleration.z = ((Mathf.PerlinNoise(it.Value.position.z*8.0f, Time.time) * 2.0f) -1.0f) * 5.0f;

                    it.Value.dragPercentage = 0.02f;
                }
                else
                {
                    it.Value.accelleration = Physics.gravity;
                it.Value.dragPercentage = 0.0f;
            }
            }

            switch (current_state)
            {
            case STATE.CASTING: // do casting in update to keep a smooth look as soaring through the sky
                {

                    LineParticles.First.Value.position = FishingLineTip.position;
                    LineParticles.Last.Value.position = fishingBob.transform.position;
                    LineParticles.Last.Value.oldPosition = fishingBob.transform.position;
                    if (Vector3.Distance(LineParticles.First.Value.position, LineParticles.First.Next.Value.position) > 0.3f)
                    {
                        LineParticle new_particle = new LineParticle();
                        new_particle.position = FishingLineTip.position;
                        new_particle.oldPosition = FishingLineTip.position;
                        LineParticles.AddFirst(new_particle);
                        GetComponent<LineRenderer>().positionCount = GetComponent<LineRenderer>().positionCount + 1;
                    }

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

                            PoleConstraint(it.Value, it.Previous.Value, 0.25f);

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

                    LineParticles.First.Value.position = FishingLineTip.position;


                     LineParticles.Last.Value.position = fishingBob.transform.position;
                     LineParticles.Last.Value.oldPosition = fishingBob.transform.position;





                    for (LinkedListNode<LineParticle> it = LineParticles.First; it != null; it = it.Next)
                    {
                        Verlet(it.Value);
                    }



                    for (int k = 0; k < accuracyItirations; k++)
                    {
                        for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                        {
                            PoleConstraint(it.Value, it.Next.Value, 0.25f);

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

                            PoleConstraint(it.Value, it.Previous.Value, 0.25f);

                        }
                    }

                    normal_line_length = 0;
                    for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                    {
                        normal_line_length += Vector3.Distance(it.Value.position, it.Next.Value.position);
                    }

                    float line_distance = 0;
                    for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                    {
                        line_distance += Vector3.Distance(it.Value.position, it.Next.Value.position);
                    }

                    if (line_distance > (Vector3.Distance(fishingBob.transform.position, FishingLineTip.position) + 0.4f))
                    {
                        LineParticles.RemoveFirst();
                        GetComponent<LineRenderer>().positionCount = LineParticles.Count;
                        LineParticles.First.Value.position = FishingLineTip.position;
                    }
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
        normal_line_length += Vector3.Distance(FishingLineTip.position, LineParticles.First.Value.position );
        normal_line_length += Vector3.Distance(LineParticles.Last.Value.position, fishingBob.transform.position);
    }

    public void BeganFighting()
    {
        current_state = STATE.FIGHTING;
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


            if (current_state == STATE.FIGHTING)
            {
                return (FishingLineTip.position - fishingBob.transform.position).normalized * extra_length * 5.0f;
            }
            else
            {
            return (FishingLineTip.position - fishingBob.transform.position).normalized * extra_length * 1.0f;
            }
        }


        return Vector3.zero;
    }
    private void Verlet(LineParticle p)
    {
        var temp = p.position;
        p.position += ((p.position - p.oldPosition)*(1.0f - p.dragPercentage)) + (p.accelleration * Time.deltaTime * Time.deltaTime);
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
