using System;

using UnityEngine;

namespace Xenon {
	public struct Line {
		public Vector3 a;
		public Vector3 b;

		public Line(Vector3 a, Vector3 b) {
			this.a = a;
			this.b = b;
		}

		public float Distance(Vector3 p) {
			Vector3 lineVec = b - a;
			return Vector3.Cross(lineVec, a - p).magnitude / lineVec.magnitude;
		}

	}

	public struct Segment {
		public Vector3 start;
		public Vector3 end;

		public Segment(Vector3 start, Vector3 end) {
			this.start = start;
			this.end = end;
		}

		public float Distance(Vector3 p) {
			Vector3 pStart = p - start;
			Vector3 lineVec = end - start;
			float h = Mathf.Clamp01(Vector3.Dot(pStart, lineVec) / Vector3.Dot(lineVec, lineVec));
			return (pStart - lineVec * h).magnitude;
		}

	}

	public struct Line2D {
		public Vector2 a;
		public Vector2 b;

		public Line2D(Vector2 a, Vector2 b) {
			this.a = a;
			this.b = b;
		}

		public float Distance(Vector2 p) {
			return Mathf.Abs((b.y - a.y) * p.x - (b.x - a.x) * p.y + b.x * a.y - b.y * a.x) / Mathf.Sqrt((b.y - a.y) * (b.y - a.y) + (b.x - a.x) * (b.x - a.x));
		}

	}

	public struct Segment2D {
		public Vector2 start;
		public Vector2 end;

		public Segment2D(Vector2 start, Vector2 end) {
			this.start = start;
			this.end = end;
		}

		public float Distance(Vector2 p) {
			Vector2 pStart = p - start;
			Vector2 lineVec = end - start;
			float h = Mathf.Clamp01(Vector2.Dot(pStart, lineVec) / Vector2.Dot(lineVec, lineVec));
			return (pStart - lineVec * h).magnitude;
		}

	}

	public struct Capsule2D {
		public Segment2D segment;
		public float radius;

		public Capsule2D(Vector2 start, Vector2 end, float radius) {
			segment = new Segment2D(start, end);
			this.radius = radius;
		}

		public Capsule2D(Segment2D segment, float radius) {
			this.segment = segment;
			this.radius = radius;
		}

		public float Distance(Vector2 p) {
			Vector2 pStart = p - segment.start;
			Vector2 lineVec = segment.end - segment.start;
			float h = Mathf.Clamp01(Vector2.Dot(pStart, lineVec) / Vector2.Dot(lineVec, lineVec));
			return (pStart - lineVec * h).magnitude - radius;
		}

	}

	public struct Circle2D {
		public Vector2 center;
		public float radius;

		public Circle2D(Vector2 center, float radius) {
			this.center = center;
			this.radius = radius;
		}

		public float Distance(Vector2 p) {
			return (p - center).magnitude - radius;
		}
	}
}
