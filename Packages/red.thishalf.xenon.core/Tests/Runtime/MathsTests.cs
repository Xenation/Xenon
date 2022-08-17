using System.Collections;

using UnityEngine;
using UnityEngine.TestTools;

using NUnit.Framework;
using Xenon;

public class MathsTests {

	static MathsTests() {
		Log.Marker("TESTS");
	}

	[Test]
	public void MathsTestsSimplePasses() {
		// Some expected float value are technically "incorrect", that is because they take into account the precission loss of float operations

		// INT
		int it = 50;
		// Max
		Assert.AreEqual(100, it.Max(100));
		Assert.AreEqual(50, it.Max(10));
		Assert.AreEqual(50, it.Max(50));
		// Min
		Assert.AreEqual(50, it.Min(100));
		Assert.AreEqual(10, it.Min(10));
		Assert.AreEqual(50, it.Min(50));
		// Abs
		Assert.AreEqual(50, 50.Abs());
		Assert.AreEqual(50, (-50).Abs());
		// Clamp
		Assert.AreEqual(10, it.Clamp(0, 10));
		Assert.AreEqual(100, it.Clamp(100, 150));
		Assert.AreEqual(50, it.Clamp(0, 100));
		
		// FLOAT
		// Abs
		Assert.AreEqual(50f, (-50f).Abs());
		Assert.AreEqual(50f, 50f.Abs());
		// Ceil
		Assert.AreEqual(51f, 50.5f.Ceil());
		Assert.AreEqual(50f, 50f.Ceil());
		// CeilToInt
		Assert.AreEqual(51, 50.5f.CeilToInt());
		Assert.AreEqual(50, 50f.CeilToInt());
		// Floor
		Assert.AreEqual(50f, 50.5f.Floor());
		Assert.AreEqual(50f, 50f.Floor());
		// FloorToInt
		Assert.AreEqual(50, 50.5f.FloorToInt());
		Assert.AreEqual(50, 50f.FloorToInt());
		// Round
		Assert.AreEqual(51f, 50.6f.Round());
		Assert.AreEqual(50f, 50.4f.Round());
		Assert.AreEqual(50f, 50.5f.Round());
		Assert.AreEqual(52f, 51.5f.Round());
		Assert.AreEqual(50f, 50f.Round());
		// RoundToInt
		Assert.AreEqual(51, 50.6f.RoundToInt());
		Assert.AreEqual(50, 50.4f.RoundToInt());
		Assert.AreEqual(50, 50.5f.RoundToInt());
		Assert.AreEqual(52, 51.5f.RoundToInt());
		Assert.AreEqual(50, 50f.RoundToInt());
		// Clamp
		Assert.AreEqual(10f, 50f.Clamp(0f, 10f));
		Assert.AreEqual(100f, 50f.Clamp(100f, 150f));
		Assert.AreEqual(50f, 50f.Clamp(0f, 100f));
		// Clamp01
		Assert.AreEqual(1f, 50f.Clamp01());
		Assert.AreEqual(0f, (-50f).Clamp01());
		Assert.AreEqual(0.5f, 0.5f.Clamp01());
		// Remap
		Assert.AreEqual(0.5f, 50f.Remap(0f, 100f, 0f, 1f));
		Assert.AreEqual(1f, 50f.Remap(25f, 50f, 0f, 1f));
		// Snap
		Assert.AreEqual(52f, 50f.Snap(52f, 5f));
		Assert.AreEqual(50f, 50f.Snap(52f, 1f));

		// VECTOR2
		Vector2 v2t = new Vector2(25f, 50f);
		Vector2 v2t2 = new Vector2(150f, 40f);
		// MaxComponent
		Assert.AreEqual(50f, v2t.MaxComponent());
		Assert.AreEqual(150f, v2t2.MaxComponent());
		// Dot
		Assert.AreEqual(1f, Vector2.up.Dot(Vector2.up));
		Assert.AreEqual(0f, Vector2.up.Dot(Vector2.right));
		Assert.AreEqual(0.70710678118f, Vector2.up.Dot(Vector2.one.normalized));
		Assert.AreEqual(-1f, Vector2.up.Dot(Vector2.down));
		// Max
		Assert.AreEqual(new Vector2(150f, 50f), v2t.Max(v2t2));
		// Min
		Assert.AreEqual(new Vector2(25f, 40f), v2t.Min(v2t2));
		// Abs
		Assert.AreEqual(new Vector2(11f, 9f), new Vector2(-11f, -9f).Abs());
		// Sign
		Assert.AreEqual(new Vector2(1f, 1f), new Vector2(11f, 800f).Sign());
		Assert.AreEqual(new Vector2(-1f, 1f), new Vector2(-99f, 50f).Sign());
		// Inverse
		Assert.AreEqual(new Vector2(0.5f, 4f), new Vector2(2f, 0.25f).Inverse());
		// Ceil
		Assert.AreEqual(new Vector2(10f, 30f), new Vector2(9.9f, 29.001f).Ceil());
		Assert.AreEqual(new Vector2(-5f, -9f), new Vector2(-5.9f, -9.001f).Ceil());
		// CeilToInt
		Assert.AreEqual(new Vector2Int(10, 30), new Vector2(9.9f, 29.001f).CeilToInt());
		Assert.AreEqual(new Vector2Int(-5, -9), new Vector2(-5.9f, -9.001f).CeilToInt());
		// Floor
		Assert.AreEqual(new Vector2(9f, 29f), new Vector2(9.9f, 29.001f).Floor());
		Assert.AreEqual(new Vector2(-6f, -10f), new Vector2(-5.9f, -9.001f).Floor());
		// FloorToInt
		Assert.AreEqual(new Vector2Int(9, 29), new Vector2(9.9f, 29.001f).FloorToInt());
		Assert.AreEqual(new Vector2Int(-6, -10), new Vector2(-5.9f, -9.001f).FloorToInt());
		// Round
		Assert.AreEqual(new Vector2(9f, 10f), new Vector2(9.1f, 9.9f).Round());
		Assert.AreEqual(new Vector2(28f, 30f), new Vector2(28.5f, 29.5f).Round());
		Assert.AreEqual(new Vector2(-9f, -10f), new Vector2(-9.1f, -9.9f).Round());
		Assert.AreEqual(new Vector2(-28f, -30f), new Vector2(-28.5f, -29.5f).Round());
		// RoundToInt
		Assert.AreEqual(new Vector2Int(9, 10), new Vector2(9.1f, 9.9f).RoundToInt());
		Assert.AreEqual(new Vector2Int(28, 30), new Vector2(28.5f, 29.5f).RoundToInt());
		Assert.AreEqual(new Vector2Int(-9, -10), new Vector2(-9.1f, -9.9f).RoundToInt());
		Assert.AreEqual(new Vector2Int(-28, -30), new Vector2(-28.5f, -29.5f).RoundToInt());
		// Step
		Assert.AreEqual(new Vector2(0f, 0f), new Vector2(10f, 15f).Step(new Vector2(20f, 20f)));
		Assert.AreEqual(new Vector2(0f, 1f), new Vector2(10f, 50f).Step(new Vector2(20f, 20f)));
		Assert.AreEqual(new Vector2(1f, 1f), new Vector2(10f, 50f).Step(new Vector2(5f, 20f)));
		// Clamp
		Assert.AreEqual(new Vector2(0f, 10f), new Vector2(-99f, 80f).Clamp(0f, 10f));
		Assert.AreEqual(new Vector2(5f, 8f), new Vector2(5f, 8f).Clamp(0f, 20f));
		Assert.AreEqual(new Vector2(2f, 5f), new Vector2(2f, 0f).Clamp(new Rect(1f, 5f, 4f, 5f)));
		Assert.AreEqual(new Vector2(2f, 10f), new Vector2(2f, 50f).Clamp(new Rect(1f, 5f, 4f, 5f)));
		Assert.AreEqual(new Vector2(1f, 8f), new Vector2(-5f, 8f).Clamp(new Rect(1f, 5f, 4f, 5f)));
		Assert.AreEqual(new Vector2(5f, 8f), new Vector2(50f, 8f).Clamp(new Rect(1f, 5f, 4f, 5f)));
		Assert.AreEqual(new Vector2(1f, 10f), new Vector2(-5f, 50f).Clamp(new Rect(1f, 5f, 4f, 5f)));
		// ClampMagnitude
		Assert.AreEqual(new Vector2(1.85695338177f, 4.64238345443f), new Vector2(4f, 10f).ClampMagnitude(0.5f, 5f));
		Assert.AreEqual(new Vector2(3f, 3f), new Vector2(3f, 3f).ClampMagnitude(0.5f, 5f));
		Assert.AreEqual(new Vector2(0.4f, 0.3f), new Vector2(0.2f, 0.15f).ClampMagnitude(0.5f, 5f));
		// Remap
		Assert.AreEqual(new Vector2(10.5f, 666f), new Vector2(21f, 1332f).Remap(0f, 1332f, 0f, 666f));
		Assert.AreEqual(new Vector2(0f, 0.5f), new Vector2(50f, 100f).Remap(50f, 150f, 0f, 1f));
		// RemapMagnitude
		Assert.AreEqual(new Vector2(0.18569533817f, 0.46423834544f), new Vector2(4f, 10f).RemapMagnitude(0f, 21.5406592285f, 0f, 1f));
		Assert.AreEqual(new Vector2(7.07106829f, 7.07106829f), new Vector2(3f, 3f).RemapMagnitude(4.24264068712f, 14.24264068712f, 10f, 20f));
		Assert.AreEqual(new Vector2(10.6066027f, 10.6066027f), new Vector2(3f, 3f).RemapMagnitude(0f, 2.82842712475f, 0f, 10f));
		// Snap
		// ToVec3
		// Unflat
		// Rotate

		// VECTOR2INT
		// MaxComponent
		// Dot
		// Max
		// Min
		// Abs
		// Sign
		// Clamp
		// ClampMaxExcluded
		// Step
		// ToVec3Int
		// Unflat
		// Float

		// VECTOR3
		// MaxComponent
		// Dot
		// Cross
		// Max
		// Min
		// Abs
		// Sign
		// Inverse
		// Ceil
		// CeilToInt
		// Floor
		// FloorToInt
		// Round
		// RoundToInt
		// Step
		// Clamp
		// ClampMagnitude
		// Remap
		// RemapMagnitude
		// Snap
		// ToVec2
		// Flat
		// ToVec4Point
		// ToVec4Dir

		// VECTOR3INT
		// MaxComponent
		// Dot
		// Max
		// Min
		// Abs
		// Sign
		// Clamp
		// ClampMaxExcluded
		// Step
		// ToVec2Int
		// Flat
		// Float

		// VECTOR4
		// MaxComponent
		// Dot
		// Max
		// Min
		// Abs
		// Sign
		// Inverse
		// Ceil
		// Floor
		// Round
		// Step
		// Clamp
		// ClampMagnitude
		// Remap
		// RemapMagnitude
		// ToVec3

		// RECT
		// SignedDistance
		// Floor
		// FloorToInt
		// Ceil
		// CeilToInt
		// Round
		// RoundToInt
		// ToIntConservative
		// Intersect
		// Bounding
		// ToNormalizedPoint
		// FromNormalizedPoint

		// RECTINT
		// Float
		// Intersect
		// Bounding
	}
}
