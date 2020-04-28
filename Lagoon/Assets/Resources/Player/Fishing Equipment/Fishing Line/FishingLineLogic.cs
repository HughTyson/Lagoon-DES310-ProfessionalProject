using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLineLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform FishingLineTip;
    [SerializeField] GameObject fishingBobLineLoop;

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


        public void Clear()
        {
            oldPosition = Vector3.zero;
            position = Vector3.zero;
            accelleration = Physics.gravity * 0.01f;
            dragPercentage = 0.01f;
        }
    }

    ObjectPooler<LineParticle> LineParticlesPool = new ObjectPooler<LineParticle>(100);


    class RodJointParticles
    {
        public GameObject connected_joint = null;
        public LineParticle particle = new LineParticle();

        public void Clear()
        {
            connected_joint = null;
            particle.Clear();
        }
    }


    ObjectPooler<RodJointParticles> RodLineParticlesPool = new ObjectPooler<RodJointParticles>(100);


    private void OnEnable()
    {
        fishingLineRenderer.material.SetColor("_UnlitColor", normal_colour);
        fishingRodDecoratorLineRenderer.material.SetColor("_UnlitColor", normal_colour);

        LineParticlesPool.DeactivateAll();

        LineParticle new_particle;
        new_particle = LineParticlesPool.ActivateObject();
        new_particle.Clear();

        new_particle.position = FishingLineTip.position;
        new_particle.oldPosition = FishingLineTip.position;


        new_particle = LineParticlesPool.ActivateObject();
        new_particle.Clear();

        new_particle.position = FishingLineTip.position;
        new_particle.oldPosition = FishingLineTip.position;


        fishingLineRenderer.positionCount = 2;



        fishing_constraint_distance = 1.0f;


        RodLineParticlesPool.DeactivateAll();

        for (int joint = 0; joint < FlexibleRodRings.Count - 1; joint++)
        {
            RodJointParticles new_joint_particle = RodLineParticlesPool.ActivateObject();
            new_joint_particle.Clear();

            new_joint_particle.particle.position = FlexibleRodRings[joint].transform.position;
            new_joint_particle.particle.oldPosition = new_joint_particle.particle.position;
            new_joint_particle.particle.accelleration = Physics.gravity * 0.1f;
            new_joint_particle.particle.dragPercentage = 0.01f;
            new_joint_particle.connected_joint = FlexibleRodRings[joint];

            for (int i = 1; i < RodDecorationLineParticlesCountPerJoint - 2; i++)
            {
                new_joint_particle = RodLineParticlesPool.ActivateObject();
                new_joint_particle.Clear();

                new_joint_particle.particle.position = Vector3.Lerp(FlexibleRodRings[joint].transform.position, FlexibleRodRings[joint + 1].transform.position, ((float)i) / ((float)(RodDecorationLineParticlesCountPerJoint - 1)));
                new_joint_particle.particle.oldPosition = new_joint_particle.particle.position;
                new_joint_particle.particle.accelleration = Physics.gravity * 0.1f;
                new_joint_particle.particle.dragPercentage = 0.01f;

            }

            new_joint_particle = RodLineParticlesPool.ActivateObject();
            new_joint_particle.Clear();

            new_joint_particle.particle.position = FlexibleRodRings[joint + 1].transform.position;
            new_joint_particle.particle.oldPosition = new_joint_particle.particle.position;
            new_joint_particle.particle.accelleration = Physics.gravity * 0.1f;
            new_joint_particle.particle.dragPercentage = 0.01f;
            new_joint_particle.connected_joint = FlexibleRodRings[joint + 1];
        }

        fishingRodDecoratorLineRenderer.positionCount = RodLineParticlesPool.ActiveObjects.Count;

        current_state = STATE.NOT_ACTIVE;
    }
    private void OnDisable()
    {
        current_state = STATE.NOT_ACTIVE;
        LineParticlesPool.DeactivateAll();
        RodLineParticlesPool.DeactivateAll();
    }
    public void ReelIn()
    {
        willReelInOnUpdate = true;
    }
    public bool IsFullyReeledIn()
    {
        return (LineParticlesPool.ActiveObjects.Count <= 1 || (willReelInOnUpdate && LineParticlesPool.ActiveObjects.Count == 2));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GM_.Instance.pause.GetPausedState() == PauseManager.PAUSED_STATE.PAUSED)
            return;

        // Collisions

        IReadOnlyList<LineParticle> lineParticles = LineParticlesPool.ActiveObjects;
        IReadOnlyList<RodJointParticles> rodLineParticles = RodLineParticlesPool.ActiveObjects;

            switch (current_state)
            {
            case STATE.CASTING: // do casting in update to keep a smooth look as soaring through the sky
                {

                    for (int i = 0; i < lineParticles.Count; i++)
                    {

                        if (lineParticles[i].position.y < GlobalVariables.WATER_LEVEL)
                        {
                            lineParticles[i].accelleration = Physics.gravity * -0.1f;
                            lineParticles[i].dragPercentage = 0.1f;
                        }
                        else
                        {
                            lineParticles[i].accelleration = Physics.gravity * 0.1f;
                            lineParticles[i].dragPercentage = 0.01f;
                        }
                    }


                    lineParticles[lineParticles.Count - 1].position = FishingLineTip.position;
                    lineParticles[0].position = fishingBobLineLoop.transform.position;
                    lineParticles[0].oldPosition = fishingBobLineLoop.transform.position;

                    setDistanceBetweenParticle0And1 = Vector3.Distance(lineParticles[lineParticles.Count - 1].position, lineParticles[lineParticles.Count - 2].position); // change distance between 

                    if (setDistanceBetweenParticle0And1 > 0.1f)
                    {
                        setDistanceBetweenParticle0And1 = 0.0001f;

                        LineParticle new_particle = LineParticlesPool.ActivateObject();
                        new_particle.position = FishingLineTip.position;
                        new_particle.oldPosition = FishingLineTip.position;
                        new_particle.accelleration = Physics.gravity * 0.1f;
                        new_particle.dragPercentage = 0.01f;

                        fishingLineRenderer.positionCount = lineParticles.Count;
                    }

                    for (int i = lineParticles.Count - 1; i >= 0; i--)
                    {
                        Verlet(lineParticles[i]);
                    }


                    for (int k = 0; k < accuracyItirations; k++)
                    {
                        lineParticles[lineParticles.Count - 1].position = FishingLineTip.position;
                        lineParticles[0].position = fishingBobLineLoop.transform.position;

                        PoleConstraint(lineParticles[lineParticles.Count - 1], lineParticles[lineParticles.Count - 2], setDistanceBetweenParticle0And1); // makes a distance transition between particle 0 and 1

                        for (int i = 0; i != lineParticles.Count - 2; i++)
                        {

                            PoleConstraint(lineParticles[i], lineParticles[i + 1], 0.1f);
                        }
                    }


                    break;
                }
            case STATE.FISHING:
                    {

                    for (int i = 0; i < lineParticles.Count; i++)
                    {

                        if (lineParticles[i].position.y < GlobalVariables.WATER_LEVEL)
                        {
                            lineParticles[i].accelleration = Physics.gravity * -0.1f;
                            lineParticles[i].dragPercentage = 0.1f;
                        }
                        else
                        {
                            lineParticles[i].accelleration = Physics.gravity * 0.1f;
                            lineParticles[i].dragPercentage = 0.01f;
                        }
                    }


                    if (willReelInOnUpdate)
                    {
                        if (lineParticles.Count != 0)
                        {
                            LineParticlesPool.DeactivateObject(lineParticles.Count - 1);
                            fishingLineRenderer.positionCount = lineParticles.Count;

                        }
                    }

                    lineParticles[lineParticles.Count - 1].position = FishingLineTip.position;
                    lineParticles[0].position = fishingBobLineLoop.transform.position;


                    for (int i = lineParticles.Count - 1; i >= 0; i--)
                    {
                        Verlet(lineParticles[i]);
                    }


                    lineParticles[lineParticles.Count - 1].position = FishingLineTip.position;
                    lineParticles[0].position = fishingBobLineLoop.transform.position;

                    for (int k = 0; k < accuracyItirations; k++)
                    {
                        for (int i = lineParticles.Count - 1; i >= 1; i--)
                        {
                            PoleConstraint(lineParticles[i], lineParticles[i - 1], fishing_constraint_distance);

                        }
                    }

                    break;
                    }
                case STATE.FIGHTING:
                    {

                    lineParticles[lineParticles.Count - 1].position = FishingLineTip.position;
                    lineParticles[0].position = fishingBobLineLoop.transform.position;


                    fishingLineRenderer.material.SetColor("_UnlitColor", Color.Lerp(snapping_colour, normal_colour, fightingFish.GetLineStrengthPercentageLeft()));
                    fishingRodDecoratorLineRenderer.material.SetColor("_UnlitColor", Color.Lerp(snapping_colour, normal_colour, fightingFish.GetLineStrengthPercentageLeft()));
                    break;
                    }


            }

        Vector3[] linePositions = new Vector3[lineParticles.Count];

        for (int i = 0; i < lineParticles.Count; i++)
        {
            linePositions[i] = lineParticles[i].position;

        }
        linePositions[0] = fishingBobLineLoop.transform.position;
        linePositions[lineParticles.Count - 1] = FishingLineTip.position;
        fishingLineRenderer.SetPositions(linePositions);

        willReelInOnUpdate = false;




        


        for (int i = 0; i < rodLineParticles.Count; i++)
        {
            Verlet(rodLineParticles[i].particle);
        }



        float second_line_constraint = 0.03f;//Mathf.Lerp(0.06f,0.03f, Mathf.Clamp(EndOfLineVelocity().magnitude,0,0.1f)/0.1f);
        if (current_state == STATE.FIGHTING)
        {
            second_line_constraint = 0.03f;
        }

        for (int k = 0; k < 5; k++)
        {
            for (int i = 0; i < rodLineParticles.Count - 1; i++)
            {
                if (rodLineParticles[i].connected_joint != null)
                {
                    rodLineParticles[i].particle.position = rodLineParticles[i].connected_joint.transform.position;
                }
                if (rodLineParticles[i + 1].connected_joint != null)
                {
                    rodLineParticles[i + 1].particle.position = rodLineParticles[i + 1].connected_joint.transform.position;
                }

                PoleConstraint(rodLineParticles[i].particle, rodLineParticles[i + 1].particle, second_line_constraint);


                if (rodLineParticles[i].connected_joint != null)
                {
                    rodLineParticles[i].particle.position = rodLineParticles[i].connected_joint.transform.position;
                }
                if (rodLineParticles[i+1].connected_joint != null)
                {
                    rodLineParticles[i+1].particle.position = rodLineParticles[i + 1].connected_joint.transform.position;
                }
            }
        }

        Vector3[] linePos = new Vector3[rodLineParticles.Count];
        for (int i = 0; i < rodLineParticles.Count; i++)
        {
            linePos[i] = rodLineParticles[i].particle.position;
        }
        fishingRodDecoratorLineRenderer.positionCount = linePos.Length;
        fishingRodDecoratorLineRenderer.SetPositions(linePos);



    }




    // -- Write Functions -- //
    public void BeganFishing()
    {
        current_state = STATE.FISHING;

        float normal_line_length = 0;

        IReadOnlyList<LineParticle> lineParticles = LineParticlesPool.ActiveObjects;

        if (lineParticles.Count > 4)
        {
            for (int i = 1; i < lineParticles.Count - 2; i++)
            {
                normal_line_length += Vector3.Distance(lineParticles[i].position, lineParticles[i+1].position);
            }

            fishing_constraint_distance = (normal_line_length / (float)(lineParticles.Count - 3))*0.9f;
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

        LineParticlesPool.DeactivateAll();

        LineParticlesPool.ActivateObject();
        LineParticlesPool.ActivateObject();
        fishingLineRenderer.positionCount = 2;


        RodLineParticlesPool.DeactivateAll();

        for (int joint = 0; joint < FlexibleRodRings.Count - 1; joint++)
        {
            RodJointParticles new_joint_particle = RodLineParticlesPool.ActivateObject();

            new_joint_particle.particle.position = FlexibleRodRings[joint].transform.position;
           
            new_joint_particle.connected_joint = FlexibleRodRings[joint];

            new_joint_particle = RodLineParticlesPool.ActivateObject();

            new_joint_particle.particle.position = FlexibleRodRings[joint + 1].transform.position;
            new_joint_particle.connected_joint = FlexibleRodRings[joint + 1];
        }

        fishingRodDecoratorLineRenderer.positionCount = RodLineParticlesPool.ActiveObjects.Count;
    }
    // -- Read Functions -- //
    public Vector3 GetEndOfLine()
    {
        return LineParticlesPool.ActiveObjects[0].position;
    }

    public Vector3 EndOfLineVelocity()
    {
        float current_rope_length = 0;
        IReadOnlyList<LineParticle> lineParticles = LineParticlesPool.ActiveObjects;

        for (int i = 0; i < lineParticles.Count - 1; i++)
        {
            current_rope_length += Vector3.Distance(lineParticles[i].position, lineParticles[i+1].position);
        }

        float fromTipToBob = Vector3.Distance(fishingBobLineLoop.transform.position, FishingLineTip.position);

        float extra_tension_multiplier = 1.01f;
        float extra_length = Mathf.Max(fromTipToBob - (current_rope_length*extra_tension_multiplier), 0);

        return (FishingLineTip.position - fishingBobLineLoop.transform.position).normalized * extra_length;
    }

    public Vector3 EndOfLineForce()
    {
        if (current_state != STATE.NOT_ACTIVE)
        {

            if (Vector3.Distance(FishingLineTip.position, fishingBobLineLoop.transform.position) > 0.01f)
            {
                IReadOnlyList<LineParticle> lineParticles = LineParticlesPool.ActiveObjects;

                float   extra_length = Vector3.Distance(FishingLineTip.position, lineParticles[lineParticles.Count - 1].position);
   
                


                switch (current_state)
                {
                    case STATE.CASTING:
                        {
                            return (FishingLineTip.position - fishingBobLineLoop.transform.position).normalized * extra_length * 10.0f;
                        }
                    case STATE.FISHING:
                        {
                            test += Time.deltaTime;
                            return (FishingLineTip.position - fishingBobLineLoop.transform.position).normalized * extra_length * 3.0f; // lerp force multiple settles the fishing rod rather than making a clear force difference
                        }
                    case STATE.FIGHTING:
                        {
                            return (FishingLineTip.position - fishingBobLineLoop.transform.position).normalized * 5.0f;

                        }
                }
            }
        }


        return Vector3.zero;
    }
    public Vector3 StartOfLinePosition()
    {
        return LineParticlesPool.ActiveObjects[LineParticlesPool.ActiveObjects.Count - 1].position;
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
