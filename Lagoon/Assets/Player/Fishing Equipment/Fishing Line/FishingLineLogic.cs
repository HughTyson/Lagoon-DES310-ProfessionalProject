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


    [SerializeField] List<GameObject> FlexibleRodRings;

    [SerializeField] int RodDecorationLineParticlesCountPerJoint;

    [SerializeField] LineRenderer fishingLineRenderer;
    [SerializeField] LineRenderer fishingRodDecoratorLineRenderer;

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

        fishingLineRenderer.positionCount = 2;
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

    class RodJointParticles
    {
        public GameObject connected_joint = null;
        public LineParticle particle = new LineParticle();
    }


    LinkedList<RodJointParticles> RodLineParticles = new LinkedList<RodJointParticles>();

    private void OnEnable()
    {
        fishingLineRenderer.material.SetColor("_UnlitColor", normal_colour);
        fishingRodDecoratorLineRenderer.material.SetColor("_UnlitColor", normal_colour);

        LineParticles.Clear();

        LineParticle new_particle = new LineParticle();
        new_particle.position = FishingLineTip.position;
        new_particle.oldPosition = FishingLineTip.position;

        LineParticle new_particle2 = new LineParticle();
        new_particle2.position = FishingLineTip.position;
        new_particle2.oldPosition = FishingLineTip.position;

        LineParticles.AddFirst(new_particle);
        LineParticles.AddFirst(new_particle2);

        fishingLineRenderer.positionCount = 2;

        fishing_constraint_distance = 1.0f;


        RodLineParticles.Clear();

        for (int joint = 0; joint < FlexibleRodRings.Count - 1; joint++)
        {
            RodJointParticles new_joint_particle = new RodJointParticles();

            new_joint_particle.particle.position = FlexibleRodRings[joint].transform.position;
            new_joint_particle.particle.oldPosition = new_joint_particle.particle.position;
            new_joint_particle.particle.accelleration = Physics.gravity * 0.1f;
            new_joint_particle.particle.dragPercentage = 0.01f;
            new_joint_particle.connected_joint = FlexibleRodRings[joint];

            RodLineParticles.AddLast(new_joint_particle);
            for (int i = 1; i < RodDecorationLineParticlesCountPerJoint - 2; i++)
            {
                RodJointParticles new_part = new RodJointParticles();
                new_part.particle.position = Vector3.Lerp(FlexibleRodRings[joint].transform.position, FlexibleRodRings[joint + 1].transform.position, ((float)i) / ((float)(RodDecorationLineParticlesCountPerJoint - 1)));
                new_part.particle.oldPosition = new_part.particle.position;
                new_part.particle.accelleration = Physics.gravity * 0.1f;
                new_part.particle.dragPercentage = 0.01f;

                RodLineParticles.AddLast(new_part);
            }

            RodJointParticles new_joint_particle_back = new RodJointParticles();

            new_joint_particle_back.particle.position = FlexibleRodRings[joint + 1].transform.position;
            new_joint_particle_back.particle.oldPosition = new_joint_particle_back.particle.position;
            new_joint_particle_back.particle.accelleration = Physics.gravity * 0.1f;
            new_joint_particle_back.particle.dragPercentage = 0.01f;
            new_joint_particle_back.connected_joint = FlexibleRodRings[joint + 1];
            RodLineParticles.AddLast(new_joint_particle_back);
        }

        fishingRodDecoratorLineRenderer.positionCount = RodLineParticles.Count;
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
                    it.Value.accelleration = Physics.gravity * -0.1f;
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
                    if (Vector3.Distance(LineParticles.First.Value.position, LineParticles.First.Next.Value.position) > 0.1f)
                    {
                        setDistanceBetweenParticle0And1 = 0.0001f;
                        LineParticle new_particle = new LineParticle();
                        new_particle.position = FishingLineTip.position;
                        new_particle.oldPosition = FishingLineTip.position;
                        LineParticles.AddFirst(new_particle);
                        fishingLineRenderer.positionCount = LineParticles.Count;
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


                    break;
                }
            case STATE.FISHING:
                    {



                    if (willReelInOnUpdate)
                    {
                        if (LineParticles.Count != 0)
                        {
                            LineParticles.RemoveFirst();
                            fishingLineRenderer.positionCount = LineParticles.Count;

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

                    LineParticles.First.Value.position = FishingLineTip.position;
                    LineParticles.Last.Value.position = fishingBob.transform.position;

                    for (int k = 0; k < accuracyItirations; k++)
                    {
                        // PoleConstraint(LineParticles.First.Value, LineParticles.First.Next.Value, setDistanceBetweenParticle0And1); // makes a distance transition between particle 0 and 1

                       

                        for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                        {
                            PoleConstraint(it.Value, it.Next.Value, fishing_constraint_distance);

                        }
                    }

                    break;
                    }
                case STATE.FIGHTING:
                    {


                    LineParticles.First.Value.position = FishingLineTip.position;
                    LineParticles.Last.Value.position = fishingBob.transform.position;
                    //LineParticles.Last.Value.oldPosition = fishingBob.transform.position;




                    //for (LinkedListNode<LineParticle> it = LineParticles.First; it != null; it = it.Next)
                    //{
                    //    Verlet(it.Value);
                    //}



                    //for (int k = 0; k < accuracyItirations; k++)
                    //{
                    //    for (LinkedListNode<LineParticle> it = LineParticles.Last; it.Previous != null; it = it.Previous)
                    //    {

                    //        LineParticles.First.Value.position = FishingLineTip.position;
                    //        LineParticles.Last.Value.position = fishingBob.transform.position;
                    //        LineParticles.Last.Value.oldPosition = fishingBob.transform.position;
                    //        PoleConstraint(it.Value, it.Previous.Value, fishing_constraint_distance);

                    //    }
                    //}



                    //while(true)
                    //{
                    //    float line_distance = 0;
                    //    for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
                    //    {
                    //        line_distance += Vector3.Distance(it.Value.position, it.Next.Value.position);
                    //    }

                    //    if (line_distance > (Vector3.Distance(fishingBob.transform.position, FishingLineTip.position) - 0.01f))
                    //    {
                    //        LineParticles.RemoveLast();
                    //        fishingLineRenderer.positionCount = LineParticles.Count;
                            
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}


                    fishingLineRenderer.material.SetColor("_UnlitColor", Color.Lerp(snapping_colour, normal_colour, fightingFish.GetLineStrengthPercentageLeft()));
                    fishingRodDecoratorLineRenderer.material.SetColor("_UnlitColor", Color.Lerp(snapping_colour, normal_colour, fightingFish.GetLineStrengthPercentageLeft()));
                    break;
                    }


            }

        Vector3[] linePositions = new Vector3[LineParticles.Count];
        int index = 0;
        for (LinkedListNode<LineParticle> it = LineParticles.First; it != null; it = it.Next)
        {

            linePositions[index] = it.Value.position;
            index++;
        }
        linePositions[0] = FishingLineTip.position;
        linePositions[LineParticles.Count - 1] = fishingBob.transform.position;
        fishingLineRenderer.SetPositions(linePositions);

        willReelInOnUpdate = false;


        for (LinkedListNode<RodJointParticles> it = RodLineParticles.First; it != null; it = it.Next)
        {
            Verlet(it.Value.particle);
        }



        float second_line_constraint = 0.03f;//Mathf.Lerp(0.06f,0.03f, Mathf.Clamp(EndOfLineVelocity().magnitude,0,0.1f)/0.1f);
        if (current_state == STATE.FIGHTING)
        {
            second_line_constraint = 0.03f;
        }

        for (int k = 0; k < 5; k++)
        {
            for (LinkedListNode<RodJointParticles> it = RodLineParticles.First; it.Next != null; it = it.Next)
            {
                if (it.Value.connected_joint != null)
                {
                    it.Value.particle.position = it.Value.connected_joint.transform.position;
                }
                if (it.Next.Value.connected_joint != null)
                {
                    it.Next.Value.particle.position = it.Next.Value.connected_joint.transform.position;
                }

                PoleConstraint(it.Value.particle, it.Next.Value.particle, second_line_constraint);


                if (it.Value.connected_joint != null)
                {
                    it.Value.particle.position = it.Value.connected_joint.transform.position;
                }
                if (it.Next.Value.connected_joint != null)
                {
                    it.Next.Value.particle.position = it.Next.Value.connected_joint.transform.position;
                }
            }
        }

        Vector3[] linePos = new Vector3[RodLineParticles.Count];
         index = 0;
        for (LinkedListNode<RodJointParticles> it = RodLineParticles.First; it != null; it = it.Next)
        {

            
            linePos[index] = it.Value.particle.position;
            index++;
        }
        fishingRodDecoratorLineRenderer.SetPositions(linePos);



    }




    // -- Write Functions -- //
    public void BeganFishing()
    {
        current_state = STATE.FISHING;

        float normal_line_length = 0;

        if (LineParticles.Count > 4)
        {
            for (LinkedListNode<LineParticle> it = LineParticles.First.Next; it.Next.Next != null; it = it.Next)
            {
                normal_line_length += Vector3.Distance(it.Value.position, it.Next.Value.position);
            }

            fishing_constraint_distance = (normal_line_length / (float)(LineParticles.Count - 3))*0.9f;
        }
        else
        {
            fishing_constraint_distance = 0.1f;
        }
    }

    public void BeganFighting(FishLogic fishLogic)
    {
        current_state = STATE.FIGHTING;
        fightingFish = fishLogic;

        LineParticles.Clear();

        LineParticle firstParticle = new LineParticle();
        LineParticle lastParticle = new LineParticle();
        LineParticles.AddFirst(firstParticle);
        LineParticles.AddLast(lastParticle);

        fishingLineRenderer.positionCount = LineParticles.Count;


        RodLineParticles.Clear();

        for (int joint = 0; joint < FlexibleRodRings.Count - 1; joint++)
        {
            RodJointParticles new_joint_particle = new RodJointParticles();

            new_joint_particle.particle.position = FlexibleRodRings[joint].transform.position;
           
            new_joint_particle.connected_joint = FlexibleRodRings[joint];

            RodLineParticles.AddLast(new_joint_particle);

            RodJointParticles new_joint_particle_back = new RodJointParticles();

            new_joint_particle_back.particle.position = FlexibleRodRings[joint + 1].transform.position;
            new_joint_particle_back.connected_joint = FlexibleRodRings[joint + 1];
            RodLineParticles.AddLast(new_joint_particle_back);
        }

        fishingRodDecoratorLineRenderer.positionCount = RodLineParticles.Count;
    }
    // -- Read Functions -- //
    public Vector3 GetEndOfLine()
    {
        return LineParticles.Last.Value.position;
    }

    public Vector3 EndOfLineVelocity()
    {
        float current_rope_length = 0;
        for (LinkedListNode<LineParticle> it = LineParticles.First; it.Next != null; it = it.Next)
        {
            current_rope_length += Vector3.Distance(it.Value.position, it.Next.Value.position);
        }

        float fromTipToBob = Vector3.Distance(fishingBob.transform.position, FishingLineTip.position);

        float extra_tension_multiplier = 1.01f;
        float extra_length = Mathf.Max(fromTipToBob - (current_rope_length*extra_tension_multiplier), 0);

        return (FishingLineTip.position - fishingBob.transform.position).normalized * extra_length;
    }

    public Vector3 EndOfLineForce()
    {
        if (current_state != STATE.NOT_ACTIVE)
        {

            if (Vector3.Distance(FishingLineTip.position, fishingBob.transform.position) > 0.01f)
            {

                 float   extra_length = Vector3.Distance(FishingLineTip.position, LineParticles.First.Value.position);
   
                


                switch (current_state)
                {
                    case STATE.CASTING:
                        {
                            return (FishingLineTip.position - fishingBob.transform.position).normalized * extra_length * 10.0f;
                        }
                    case STATE.FISHING:
                        {
                            test += Time.deltaTime;
                            return (FishingLineTip.position - fishingBob.transform.position).normalized * extra_length * 3.0f; // lerp force multiple settles the fishing rod rather than making a clear force difference
                        }
                    case STATE.FIGHTING:
                        {
                            return (FishingLineTip.position - fishingBob.transform.position).normalized * 5.0f;

                        }
                }
            }
        }


        return Vector3.zero;
    }
    public Vector3 StartOfLinePosition()
    {
        return LineParticles.First.Value.position;
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
