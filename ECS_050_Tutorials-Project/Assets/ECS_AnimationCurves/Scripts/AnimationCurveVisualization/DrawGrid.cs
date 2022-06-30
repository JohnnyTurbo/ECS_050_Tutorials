using System.Collections;
using System.Collections.Generic;
using Shapes;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.AnimationCurveVisualization
{
    enum PointMode
    {
        Step,
        Lerp
    }
    
    [ExecuteAlways]
    public class DrawGrid : ImmediateModeShapeDrawer
    {
        [SerializeField] private float _mainLineThickness;
        [SerializeField] private float _subLineThickness;
        [SerializeField] private Vector2 _gridSize;
        [SerializeField] private bool _drawSubLines;
        [SerializeField] private bool _drawSamplePoints;
        [SerializeField] private bool _drawPolyLine;
        [SerializeField] private bool _drawSamplePoint;
        [SerializeField] private int _samplePointCount;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private int _visualFidelity;
        [SerializeField] private float _samplePoint;
        [SerializeField] private PointMode _pointMode;
        [SerializeField] private float _traversalSpeed;

        private float _traversalTimer = 0f;
        
        private void Start()
        {
            //StartCoroutine(TraverseGraph());
        }

        private IEnumerator TraverseGraph()
        {
            while (true)
            {
                var traversalTime = _traversalTimer / _traversalSpeed;
                var traversalPoint = traversalTime * _gridSize.x;

                _samplePoint = traversalPoint;

                _traversalTimer += Time.deltaTime;
                _traversalTimer %= _traversalSpeed;

                yield return null;
            }
        }
        
        [ExecuteAlways]
        public override void DrawShapes(Camera cam)
        {
            using (Draw.Command(cam))
            {
                Draw.LineGeometry = LineGeometry.Volumetric3D;
                Draw.ThicknessSpace = ThicknessSpace.Meters;
                Draw.Matrix = transform.localToWorldMatrix;
                Draw.Thickness = _mainLineThickness;
                Draw.Color = new Color(0.85f, 0.85f, 0.85f);

                var corner0 = Vector3.zero;
                var corner1 = Vector3.right * _gridSize.x;
                var corner2 = Vector3.up * _gridSize.y;
                var corner3 = corner1 + corner2;
                
                Draw.Line(corner0, corner1);
                Draw.Line(corner0, corner2);
                Draw.Line(corner1, corner3);
                Draw.Line(corner2, corner3);

                Draw.Thickness = _subLineThickness;

                if (_drawSubLines)
                {
                    var horizontalModifier = _samplePointCount - 1;
                    for (var i = 0; i < _samplePointCount - 1; i++)
                    {
                        var xPos = _gridSize.x / horizontalModifier * (i + 1);
                        var bottom = new Vector3(xPos, 0f, 0f);
                        var top = new Vector3(xPos, _gridSize.y, 0f);
                        Draw.Line(bottom, top);
                    }
                }
                
                using (var p = new PolylinePath())
                {
                    for (var i = 0; i < _visualFidelity; i++)
                    {
                        var x = (float)i / (_visualFidelity - 1);
                        var y = _animationCurve.Evaluate(x);
                        x *= _gridSize.x;
                        y *= _gridSize.y;
                        p.AddPoint(x,y);
                    }
                    Draw.Polyline(p, closed:false, thickness:0.1f, Color.green);
                }
                
                using (var p = new PolylinePath())
                {
                    var mainPoints = new List<Vector3>();
                    for (var i = 0; i < _samplePointCount; i++)
                    {
                        var x = (float)i / (_samplePointCount - 1);
                        var y = _animationCurve.Evaluate(x);
                        x *= _gridSize.x;
                        y *= _gridSize.y;
                        p.AddPoint(x,y);
                        mainPoints.Add(new Vector3(x,y));
                        if (_pointMode == PointMode.Step && i < _samplePointCount - 1)
                        {
                            var nextX = (float)(i + 1) / (_samplePointCount - 1) * _gridSize.x;
                            
                            p.AddPoint(nextX, y);
                        }
                    }

                    if (_drawPolyLine)
                    {
                        Draw.Polyline(p, closed:false, thickness:0.1f, Color.red);
                    }

                    if (_drawSamplePoints)
                    {
                        foreach (var point in mainPoints)
                        {
                            Draw.Disc(point, 0.15f, DiscColors.Flat(new Color(1f, 0.647f, 0f)));
                        }
                    }
                    
                    if (_samplePoint >= 0f && _samplePoint <= _gridSize.x)
                    {
                        var start = new Vector3(_samplePoint, 0f, 0f);
                        var end = new Vector3(_samplePoint, _gridSize.y, 0f);
                        Draw.Line(start, end, Color.yellow);

                        var approxIndex = (_samplePointCount - 1) * (_samplePoint/_gridSize.x);
                        var indexBelow = (int)Mathf.Floor(approxIndex);
                        if (_drawSamplePoint)
                        {
                            Vector3 sampledPoint;
                            if (indexBelow >= _samplePointCount - 1)
                            {
                                sampledPoint = mainPoints[_samplePointCount - 1];
                            }
                            else
                            {
                                var indexRemainder = approxIndex - indexBelow;
                                if (_pointMode == PointMode.Lerp)
                                {
                                    sampledPoint = Vector3.Lerp(mainPoints[indexBelow], mainPoints[indexBelow + 1], indexRemainder);
                                }
                                else
                                {
                                    sampledPoint = new Vector3(_samplePoint, mainPoints[indexBelow].y);
                                }
                            }
                            Draw.Disc(sampledPoint, 0.2f, DiscColors.Flat(new Color(0.672f, 0f, 1f)));
                        }
                    }
                }
            }
        }

        private void OnValidate()
        {
            _samplePointCount = math.max(2, _samplePointCount);
        }
    }
}