using UnityEngine;

namespace Xenon {
	public static class Utils {

		public static Transform2D To2D(this Transform transform) {
			return new Transform2D(transform);
		}

		public static float Rotation2D(this Transform transform) {
			return Vector2.SignedAngle(Vector2.right, transform.right.ToVec2());
		}

		public static float LocalRotation2D(this Transform transform) {
			return Vector2.SignedAngle((transform.parent == null) ? Vector2.right : transform.parent.right.ToVec2(), transform.right.ToVec2());
		}

		public static void SetRotation2D(this Transform transform, float rotation) {
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotation);
		}

		public static void SetLocalRotation2D(this Transform transform, float localRotation) {
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, localRotation);
		}

		public static Vector2 Position2D(this Transform transform) {
			return transform.position.ToVec2();
		}

		public static Vector2 LocalPosition2D(this Transform transform) {
			return transform.localPosition.ToVec2();
		}

		public static void SetPosition2D(this Transform transform, Vector2 position) {
			transform.position = position.ToVec3(transform.position.z);
		}

		public static void SetLocalPosition2D(this Transform transform, Vector2 localPosition) {
			transform.localPosition = localPosition.ToVec3(transform.localPosition.z);
		}

		public static Vector2 Scale2D(this Transform transform) {
			return transform.lossyScale.ToVec2();
		}

		public static Vector2 LocalScale2D(this Transform transform) {
			return transform.localScale.ToVec2();
		}

		public static void SetLocalScale2D(this Transform transform, Vector2 localScale) {
			transform.localScale = localScale.ToVec3(transform.localScale.z);
		}

	}
}
