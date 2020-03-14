using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic
{
    public class Rule // set up output shape here and calculate moment here
    {

        struct SimplerLineSegment
        {
            public SimplerLineSegment(float x_value_, float membership_amm_)
            {
                x_value = x_value_;
                membership_amm = membership_amm_;
            }
            public float membership_amm;
            public float x_value;
        }

        List<SimplerLineSegment> lineSegments = new List<SimplerLineSegment>();

        public struct AreaAndMoment
        {
            public float area;
            public float moment;
        }

    
        public void AddPointToOutputDomain(float x_value_, float membership_amm_)
        {
            membership_amm_ = Mathf.Clamp01(membership_amm_);
            lineSegments.Add(new SimplerLineSegment(x_value_, membership_amm_));
        }

        public AreaAndMoment CalculateAreaAndMomentValue(float cuttoff)
        {
            AreaAndMoment areaAndMoment = new AreaAndMoment();
            areaAndMoment.area = 0;
            areaAndMoment.moment = 0;
            for (int i = 1; i < lineSegments.Count; i++)
            {
                float local_area = 0;
                float local_moment = 0;


                SimplerLineSegment line_start = lineSegments[i - 1];
                SimplerLineSegment line_end = lineSegments[i];


                float min_y_point = Mathf.Min(line_start.membership_amm, line_end.membership_amm);
                if (cuttoff > min_y_point) // triangle is part of the area
                {
                    float height = (cuttoff - min_y_point); // height
                    Vector2 end_to_start = (new Vector2(line_start.x_value, line_start.membership_amm) - new Vector2(line_end.x_value, line_end.membership_amm)).normalized;

                    Vector2 angle_dir = (end_to_start.y < 0) ? end_to_start : -end_to_start;
                    float angle = Vector2.Angle(angle_dir, Vector2.down);
                    float triag_width = Mathf.Abs(Mathf.Tan(Mathf.Deg2Rad * angle) * height); // length of base

                    float triag_area = height * triag_width * 0.5f;
                    float triag_x_centriod;

                    if (end_to_start.y < 0) // finds which side the 90 degree corner is
                    {
                        triag_x_centriod = ((line_start.x_value) + triag_width) - (triag_width / 3.0f);
                    }
                    else
                    {
                        triag_x_centriod = ((line_end.x_value) - triag_width) + (triag_width / 3.0f);
                    }

                    local_area += triag_area;
                    local_moment += triag_area * triag_x_centriod;

                    float small_square_width = ((line_end.x_value - line_start.x_value)) - triag_width;

                    if (small_square_width > 0.0001f)
                    {
                        float small_square_area = small_square_width * height;
                        float small_square_x_centriod = line_start.x_value + small_square_width * 0.5f;
                        local_area += small_square_area;
                        local_moment += small_square_area * small_square_x_centriod;
                    }

                }
                float square_area = Mathf.Min(cuttoff, min_y_point) * (line_end.x_value - line_start.x_value);
                float square_x_centriod = line_start.x_value + (line_end.x_value - line_start.x_value) * 0.5f;

                local_area += square_area;
                local_moment += square_area * square_x_centriod;


                areaAndMoment.area += local_area;
                areaAndMoment.moment += local_moment;
            }
            return areaAndMoment;
        }
    }
    public class MembershipFunc
    {
        public enum LINE_TYPE
        {
            LINEAR,
            EASE_IN,
            EASE_OUT,
            EASE_IN_OUT,
            NON_EASE_IN_OUT
        };

        struct LineSegment
        {
            public LineSegment(float x_value_, float membership_amm_, LINE_TYPE line_type_)
            {
                x_value = x_value_;
                membership_amm = membership_amm_;
                line_type = line_type_;
            }
            public float membership_amm;
            public float x_value;
            public LINE_TYPE line_type;
        }

        List<LineSegment> lineSegments = new List<LineSegment>();


        public void StartDomain(float x_value_, float membership_amm_)
        {
            membership_amm_ = Mathf.Clamp01(membership_amm_);
            lineSegments.Add(new LineSegment(x_value_, membership_amm_, LINE_TYPE.LINEAR));
        }

        public void AddPointToDomain(float x_value_, float membership_amm_, LINE_TYPE line_type) // line type is the type of line between this point and the previous point
        {
            membership_amm_ = Mathf.Clamp01(membership_amm_);
            lineSegments.Add(new LineSegment(x_value_, membership_amm_, line_type));
        }

        public float CalculateMembershipAmount(float input_val)
        {
            if (input_val <= lineSegments[0].x_value)
            {
                return lineSegments[0].membership_amm;
            }
            for (int i = 1; i < lineSegments.Count; i++)
            {
                float line_start = lineSegments[i - 1].x_value;
                float line_end = lineSegments[i].x_value;

                if (input_val >= line_start && input_val <= line_end)
                {
                    float t = (input_val - line_start) / (line_end - line_start);

                    switch (lineSegments[i].line_type)
                    {
                        case LINE_TYPE.LINEAR:
                            {
                                // t = t
                                break;
                            }
                        case LINE_TYPE.EASE_IN:
                            {
                                t = t * t;
                                break;
                            }
                        case LINE_TYPE.EASE_OUT:
                            {
                                t = (1.0f - ((1.0f - t) * (1.0f - t)));
                                break;
                            }
                        case LINE_TYPE.EASE_IN_OUT: // ease in out function from Perlin Noise
                            {
                                t = (3.0f * (t * t)) - (2.0f * (t * t * t));
                                break;
                            }
                        case LINE_TYPE.NON_EASE_IN_OUT:
                            {
                                float inner_value = (t - 0.5f) * 1.6f;
                                t = ((inner_value * inner_value * inner_value) + 0.5f);
                                break;
                            }
                    }
                    return Mathf.Lerp(lineSegments[i - 1].membership_amm, lineSegments[i].membership_amm, t);
                }
            }
            return lineSegments[lineSegments.Count - 1].membership_amm;
        }


        public void DEBUG_DRAW_DOMAIN(object key, int total_samples)
        {
            float total_length = lineSegments[lineSegments.Count - 1].x_value - lineSegments[0].x_value;
            for (int i = 1; i < lineSegments.Count; i++)
            {
                float line_start = lineSegments[i - 1].x_value;
                float line_end = lineSegments[i].x_value;

                float percentage_of_samples = ((line_end - line_start) / total_length);

                int samples_this_line_segment = (int)(percentage_of_samples * (float)total_samples);
                for (int samp = 0; samp < samples_this_line_segment; samp++)
                {
                    float t = (float)(samp) / Mathf.Max((float)(samples_this_line_segment - 1), 1);

                    float input_amm = Mathf.Lerp(line_start, line_end, t);


                    switch (lineSegments[i].line_type)
                    {
                        case LINE_TYPE.LINEAR:
                            {
                                // t = t
                                break;
                            }
                        case LINE_TYPE.EASE_IN:
                            {
                                t = t * t;
                                break;
                            }
                        case LINE_TYPE.EASE_OUT:
                            {
                                t = (1.0f - ((1.0f - t) * (1.0f - t)));
                                break;
                            }
                        case LINE_TYPE.EASE_IN_OUT: // ease in out function from Perlin Noise
                            {
                                t = (3.0f * (t * t)) - (2.0f * (t * t * t));
                                break;
                            }
                        case LINE_TYPE.NON_EASE_IN_OUT:
                            {
                                float inner_value = (t - 0.5f) * 1.6f;
                                t = ((inner_value * inner_value * inner_value) + 0.5f);
                                break;
                            }
                    }

                    float membership_am = Mathf.Lerp(lineSegments[i - 1].membership_amm, lineSegments[i].membership_amm, t);

                    DebugGUI.Graph(key, membership_am);

                }
            }


        }
    }
}
